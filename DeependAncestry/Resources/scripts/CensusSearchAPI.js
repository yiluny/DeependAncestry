var CensusSearchAPI = (function () {
    var loadCensusData = function () {
        var itemPerPage = 10;
        var pageNumber = 0;
        var gender = "all";
        var family = "null";

        var keyword = document.getElementById("searched-text").value;
        var maleCheckBox = document.getElementById("male-checkox").checked;
        var femaleCheckBox = document.getElementById("female-checkox").checked;
        var familyOptionRadioSelections = document.getElementsByName("family");

        if (!maleCheckBox || !femaleCheckBox) {
            if (maleCheckBox) gender = "m";
            if (femaleCheckBox) gender = "f";
        }

        for (i = 0; i < familyOptionRadioSelections.length; i++) {
            if (familyOptionRadioSelections[i].checked) {
                family = familyOptionRadioSelections[i].value;
            }
        }

        $.ajax({
            url: "/api/search/search/" + keyword + "/" + gender + "/" + family + "/" + pageNumber + "/"+ itemPerPage,
            jsonp: "callback",
            dataType: "json",
            success: function (data) {
                //var template = $('#template').html();
                //Mustache.parse(template);
                //var rendered = Mustache.render(template, data);
                //$('.news-list .news-posts').append(rendered);
                //$("#result-list").load("/home/DisplayResultPartialView", { People: data.Results })
                if (data.Results.length > 0) {
                    $("#people-result").empty();
                    data.Results.forEach(function (person) {
                        $("#people-result").append(
                            '<tr><td>' + person.ID + '</td><td>' + person.Name + '</td><td>' + person.Gender + '</td><td>' + person.BirthPlace + '</td></tr>');
                    }, this);
                }
            }
        });

    }

    // Public API
    return {
        loadCensusData: loadCensusData
    };
})();