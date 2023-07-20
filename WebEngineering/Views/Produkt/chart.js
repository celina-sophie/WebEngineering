var ctx = document.getElementById('barChart').getContext('2d');

@if (ViewBag.ChartData != null) {
    var labels = @Html.Raw(Json.Serialize(ViewBag.ChartData.Labels));
    var durchschnittlicheBestellmenge = @Html.Raw(Json.Serialize(ViewBag.ChartData.DurchschnittlicheBestellmenge));
    var durchschnittlicheLiefermenge = @Html.Raw(Json.Serialize(ViewBag.ChartData.DurchschnittlicheLiefermenge));

    var barChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Durchschnittliche Bestellmenge',
                    backgroundColor: 'red',
                    borderColor: 'red',
                    borderWidth: 1,
                    data: durchschnittlicheBestellmenge,
                },
                {
                    label: 'Durchschnittliche Liefermenge',
                    backgroundColor: 'blue',
                    borderColor: 'blue',
                    borderWidth: 1,
                    data: durchschnittlicheLiefermenge,
                }
            ]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}
