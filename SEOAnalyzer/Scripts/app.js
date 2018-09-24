$(function () {
    $('#analyzeTextButton').click(analyzeTextClick);
    $('#analyzeUrlButton').click(analyzeUrlClick);

    function analyzeTextClick() {
        var text = $('#plainTextArea').val();

        if (text) {
            $.ajax({
                type: 'POST',
                url: 'api/WordCount/GetWordCountByText',
                data: { 'source': text },
                dataType: 'json',
                success: function (data) {
                    $('#textAnalysisForm').addClass("hidden");
                    $('#wordResultPanel').removeClass("hidden");
                    $('#back').removeClass("hidden");

                    if (data) {
                        initializeTable(data.Words, 'textResult');
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
        }

        return false;
    }

    function analyzeUrlClick() {
        var url = $('#sourceUrl').val();

        if (url) {
            $.ajax({
                type: 'POST',
                url: 'api/WordCount/GetWordCountByUrlAsync',
                data: { 'source': url },
                dataType: 'json',
                success: function (data) {
                    $('#urlAnalysisForm').addClass("hidden");
                    $('#wordResultPanel').removeClass("hidden");
                    $('#metaKeyordResultPanel').removeClass("hidden");
                    $('#linkResultPanel').removeClass("hidden");
                    $('#back').removeClass("hidden");

                    if (data) {
                        initializeTable(data.Words, 'urlWordResult');
                        initializeTable(data.MetaKeywords, 'metaKeywordResult');
                        initializeTable(data.ExternalLinks, 'linkResult');
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
        }

        return false;
    }

    function initializeTable(object, tableId) {
        var rows = [];

        if (Array.isArray(object)) {
            for (var link in object) {
                var row = '<tr><th>' + object[link] + '</th></tr>';

                rows.push(row);
            }
        }
        else {
            for (var property in object) {
                if (object.hasOwnProperty(property)) {
                    var row = '<tr><th>' + property + '</th><th>' + object[property] + '</th></tr>';

                    rows.push(row);
                }
            }
        }
        

        $('#' + tableId + ' tbody').empty()
        $('#' + tableId + ' > tbody:last-child').append(rows.join(''));
        $('#' + tableId).footable();
    }
});