window.Candidate = Backbone.Model.extend({
    urlRoot: "/api/Candidate",
    defaults: {
        "Id": null,
        "RecruiterId": null,
        "Name": "",
        "RecruiterName": null,
    },
    idAttribute: 'Id'
});

window.CandidateCollection = Backbone.Collection.extend({
    model: Candidate,
    url: "/api/Candidate"
});


window.CandidateListView = Backbone.View.extend({

    tagName: 'ul',

    initialize: function () {
        this.model.bind("reset", this.render, this);
        var self = this;
        this.model.bind("add", function (Candidate) {
            $(self.el).append(new CandidateListItemView({ model: Candidate }).render().el);
        });
    },

    render: function (eventName) {
        $(this.el).html("");
        _.each(this.model.models, function (Candidate) {
            $(this.el).append(new CandidateListItemView({ model: Candidate }).render().el);
        }, this);
        return this;
    }
});


window.CandidateListItemView = Backbone.View.extend({

    tagName: "li",

    template: _.template($('#tpl-candidate-list-item').html()),

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


window.CandidateView = Backbone.View.extend({
    template: _.template($('#tpl-candidate-details').html()),
    initialize: function () {
        this.model.bind("change", this.render, this);
    },

    render: function (eventName) {
        $(this.el).html(this.template(this.model.toJSON()));
        var childTemplate = _.template($('#tpl-recruiter-selector').html(), {
            recruiterList: app.recruiterList.toJSON()
        });
        console.log("Rendering");
        $(this.el).find("#RecruiterPlaceholder").html(childTemplate);
        if (this.model.get("RecruiterId")) {
            $(this.el).find("#RecruiterPlaceholder select").val(this.model.get("RecruiterId"));
        }
        return this;
    },

    events: {
        "click .save": "saveCandidate",
        "click .delete": "deleteCandidate"
    },

    saveCandidate: function () {

        var name = $('#name').val();
        //TO DO: use model based validations
        if (name == "" || name == null) {
            alert("Name cannot be blank");
            return false;

        }
        else {
            var recruiterId = $('#recruiterId').val();



            if (recruiterId == "") {
                recruiterId = null;
            }
            this.model.set({
                Name: name,
                RecruiterId: recruiterId
            });
            if (this.model.isNew()) {
                app.candidateList.create(this.model)
            } else {
                this.model.save();
            }
            return false;
        }
    },

    deleteCandidate: function () {
        this.model.destroy({
            success: function () {
                alert('Candidate deleted successfully');
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
