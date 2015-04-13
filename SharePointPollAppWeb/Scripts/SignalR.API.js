var SignalR_API = {

	 

    initReceiver: function () {
        var theHub = $.connection.theHub;
        theHub.client.showNotifiedVotes = function showNotifiedVotes(user, data) {

			console.log(user);
			console.log(data);
			
			//make ajax call to get particular survey chart : set default surveyId to load or replace the exact div  with the new chart and show that div 
			var surveyId = data.SurveyId;

			$.ajax({
				type: 'GET',
				data: { SurveyId: surveyId },
				url: "SurveyChart/GetSurveyChart",
				success: function (data, textStatus, jqXHR) {
					$(".surveyChartWrapper#" + surveyId).empty().html(data);
					$(".surveyChartWrapper:Visible").hide();
					$(".surveyChartWrapper#" + surveyId).show();
					var nextSurveyId = parseInt(surveyId) + 1;
					if ($(".surveyChartWrapper#" + nextSurveyId).length > 0)
						$("#btnNext").show();
					else
						$("#btnNext").hide();
				},
				error: function (jqXHR, textStatus, errorThrown) {
					console.log('Error: ' + errorThrown);
				}
			});

			var encodedMsg = $('<div />').text(user + " just voted for " + data.SelectedAnswer ).html();
			toastr.warning(encodedMsg);
			
		};
    },

	initSender: function (groupName) {
		var self = this;
		$('.submitVote').click(function () {
			// Call the Send method on the hub.

			var surveyId = $(".survey:Visible").attr('id');
			var currentSurveyDiv = $(".survey#" + surveyId);
			var selectedAnswer = $(currentSurveyDiv.find($("input[name='SelectedAnswer']:checked"))).val();

			$.ajax({
				type: 'POST',
				data: { SurveyId: surveyId, SelectedAnswer: selectedAnswer },
				url: "Survey/SubmitVote",
				success: function (data, textStatus, jqXHR) {
				    debugger;
				    var theHub = $.connection.theHub;
				    if (groupName) {
				        theHub.server.notifyVotesToGroup(data.user, data.data, groupName);
				    }
				    else {
				        theHub.server.notifyVotes(data.user, data.data);
				    }
				   
				},
				error: function (jqXHR, textStatus, errorThrown) {
					console.log('Error: ' + errorThrown);
				}
			});
		});
	},

	addToGroup: function (groupName)
	{
	    var theHub = $.connection.theHub;
	    theHub.server.addToGroup(groupName);
	},

	startConnection: function (callback, callback_param) {
		$.connection.hub.start().done(function () {
			console.log("connection started");
			if (callback) {
			    if (callback_param) {
			        callback(callback_param);
			    }
			    else {
			        callback();
			    }
			}
		});
	}
}