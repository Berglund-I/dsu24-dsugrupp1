// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showStatistics () {
    const ctx = document.getElementById('myChart').getContext('2d');
    const data = {
        labels: ['1 Dos', '2 Doser', '4 Doser eller fler', 'Ingen dos'],
        datasets: [{
            label: 'Vaccinationsgraden i %',
            data: [80, 75, 68, 54],
            backgroundColor: ['rgb(220, 174, 198)', 'rgb(162, 221, 109)', 'rgb(255, 239, 107)', 'rgb(0, 0, 0)'],
            borderWidth: 1
        }]
    };
    const options = {
        scales: {
            y: {
                beginAtZero: true,
                ticks: {
                    stepSize: 5,
                    min: 0,
                    max: 100
                }
            }
        }
    };

    
    new Chart(ctx, {
        type: 'bar',
        data: data,
        options: options
    });
}


document.addEventListener('DOMContentLoaded', (event) => {
    showStatistics();
});




let totalVaccinated = document.getElementById('totalVaccinations').value;
console.log(totalVaccinated);

document.addEventListener('DOMContentLoaded', (event) => {
    showDeSo();
});

/**Function that creates and presents a pie-chart */
function showDeSo() {
    const context = document.getElementById('deSoChart').getContext('2d');
    var xValues = ["Vaccinerade med minst en dos", "Ovaccinerade"];
    var yValues = [totalVaccinated, 40];
    var barColors = [
        "#b91d47",
        "#00aba9"
    ];

    new Chart(context, {
        type: "doughnut",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValues
            }]
        },
        options: {
            title: {
                display: true,
                text: "Vaccinationsgrad i DeSo-området: "
            }
        }
    });

}

