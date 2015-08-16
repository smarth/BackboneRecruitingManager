
window.Recruiter = Backbone.Model.extend({
    urlRoot: "/api/Recruiter",
    defaults: {
        "Id": null,
        "Name": ""
    },
    idAttribute: 'Id'
});


window.RecruiterCollection = Backbone.Collection.extend({
    model: Recruiter,
    url: "/api/Recruiter"
});



window.RecruiterListView = Backbone.View.extend({

    tagName: 'ul',

    initialize: function () {
        this.model.bind("reset", this.render, this);
        var self = this;
        this.model.bind("add", function (Recruiter) {
            $(self.el).append(new RecruiterListItemView({ model: Recruiter }).render().el);
        });
    },

    render: function (eventName) {
        $(this.el).html("");
        _.each(this.model.models, function (Recruiter) {
            $(this.el).append(new RecruiterListItemView({ model: Recruiter }).render().el);
        }, this);
        return this;
    }
});


window.RecruiterListItemView = Backbone.View.extend({

    tagName: "li",

    template: _.template($('#tpl-recruiter-list-item').html()),

    initialize: function () {
        this.model.bind("change", this.render, this);
        this.model.bind("destroy", this.close, this);
    },

    render: function (eventName) {
        $(this.el).html(this.template(this.model.toJSON()));
        return this;
    },

    close: function () {
        $(this.el).unbind();
        $(this.el).remove();
    }
});


window.RecruiterView = Backbone.View.extend({
    template: _.template($('#tpl-recruiter-details').html()),
    initialize: function () {
        this.model.bind("change", this.render, this);
    },

    render: function (eventName) {
        $(this.el).html(this.template(this.model.toJSON()));
        var id = this.model.get("Id");
        var candidateList = app.candidateList.filter(function (candidate) {
            return (id != null && candidate.get("RecruiterId") == id);
        });
        var candidateListCollection = new CandidateCollection();
        for (var i = 0; i < candidateList.length; ++i) {

            candidateListCollection.add(candidateList[i]);
        }

        var candidateListView = new CandidateListView({ model: candidateListCollection });
        if (candidateList.length > 0) {
            $(this.el).append("Candidates Assigned To This Recruiter");
        }
        $(this.el).append(candidateListView.render().el);


        return this;
    },

    events: {
        "click .save": "saveRecruiter",
        "click .delete": "deleteRecruiter"
    },

    saveRecruiter: function () {
        var name = $('#name').val();
        //TO DO: use model based validations
        if (name == "" || name == null) {
            alert("Name cannot be blank");
            return false;

        }
        else {
            this.model.set({
                Name: name
            });
            if (this.model.isNew()) {
                app.recruiterList.create(this.model);
            } else {
                this.model.save();
            }
            return false;
        }
    },

    deleteRecruiter: function () {
        this.model.destroy({
            success: function () {
                alert('Recruiter deleted successfully');

                app.candidateList.fetch({ add: false, reset: true });
                window.history.back();
            },
            error: function () {
                console.log("Error");
            }
        });
        return false;
    },

    close: function () {
        $(this.el).unbind();
        $(this.el).empty();
    }
});

