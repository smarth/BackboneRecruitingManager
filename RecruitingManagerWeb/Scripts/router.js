var AppRouter = Backbone.Router.extend({

    routes: {
        "": "list",
        "Candidates/:Id": "candidateDetails",
        "Recruiters/:Id": "recruiterDetails"
    },

    list: function () {
        this.candidateList = new CandidateCollection();
        this.candidateListView = new CandidateListView({ model: this.candidateList });
        this.recruiterList = new RecruiterCollection();
        this.recruiterListView = new RecruiterListView({ model: this.recruiterList });
        var self = this;
        this.recruiterList.fetch({
            success: function () {
                self.candidateList.fetch(
                    {
                        success: function () {
                            $('#sidebar #recruitersList').html(self.recruiterListView.render().el);
                            $('#sidebar #candidatesList').html(self.candidateListView.render().el);
                            if (self.candidateRequested) {
                                self.candidateDetails(self.requestedId);
                            }

                            if (self.recruiterRequested) {
                                self.recruiterDetails(self.requestedId);
                            }
                        }
                    }
                );
            }
        });




    },

    candidateDetails: function (Id) {
        if (this.candidateList) {
            this.candidate = this.candidateList.get(Id);
            if (app.candidateView) app.candidateView.close();
            if (app.recruiterView) app.recruiterView.close();
            this.candidateView = new CandidateView({ model: this.candidate });
            $('#content').html(this.candidateView.render().el);
        }
        else {
            this.candidateRequested = true;
            this.recruiterRequested = false;
            this.requestedId = Id;
            this.list();
        }
    },

    recruiterDetails: function (Id) {
        if (this.recruiterList) {
            this.recruiter = this.recruiterList.get(Id);
            if (app.candidateView) app.candidateView.close();
            if (app.recruiterView) app.recruiterView.close();
            this.recruiterView = new RecruiterView({ model: this.recruiter });
            $("#content").html(this.recruiterView.render().el);
        }
        else {
            this.candidateRequested = false;
            this.recruiterRequested = true;
            this.requestedId = Id;
            this.list();
        }
    }


});

var app = new AppRouter();
Backbone.history.start();
