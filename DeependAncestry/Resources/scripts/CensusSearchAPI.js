var CensusSearchAPI = (function () {
    var itemPerPage = 10;
    var pageIndex = 0;
    var totalCount = 0;

    //Pagination
    var loadPage = function (isNext) {
        if (isNext) pageIndex++;
        else pageIndex--;

        loadCensusData();
    }

    //load data from api
    var loadCensusData = function () {
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
            url: "/api/search/search/" + keyword + "/" + gender + "/" + family + "/" + pageIndex + "/" + itemPerPage,
            jsonp: "callback",
            dataType: "json",
            success: function (data) {
                if (data.Results.length > 0) {
                    $("#people-result").empty();

                    totalCount = data.TotalCount;

                    //Check if show or hide the pagination buttons
                    if ((pageIndex + 1) * itemPerPage < totalCount) $("#nxt-page-btn").show();
                    else $("#nxt-page-btn").hide();
                    if (pageIndex == 0) $("#pre-page-btn").hide();
                    else $("#pre-page-btn").show();

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
        loadCensusData: loadCensusData,
        loadPage: loadPage
    };
})();