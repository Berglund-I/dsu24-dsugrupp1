// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let leftChart;
let rightChart;
let leftGenderChart;
let rightGenderChart;

document.addEventListener('DOMContentLoaded', function () {
    const deSoDropdown = document.getElementById('left-deSo-dropdown');

    deSoDropdown.addEventListener('change', function () {
        let selectedDeSo = this.value;

        fetch('/Home/GetChartFromDeSoCode', {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',

            },
            body: JSON.stringify({ selectedDeSo }),
        })

            .then(response => {
                if (!response.ok) {
                    throw new Error('Inte bra');
                }
                return response.json();
            })
            .then(data => {


                clearDeSoInformation('left-deSo-statistics');
                const deSoStatisticContainer = document.getElementById('left-deSo-statistics');

                const population = document.createElement('p');
                population.innerText = 'Invånare i området: ' + data.population;
                deSoStatisticContainer.appendChild(population);

                const doseOne = document.createElement('p');
                doseOne.innerText = 'Antal dos 1 injektioner: ' + data.doseOne;
                deSoStatisticContainer.appendChild(doseOne)

                const doseTwo = document.createElement('p');
                doseTwo.innerText = 'Antal dos 2 injektioner: ' + data.doseTwo;
                deSoStatisticContainer.appendChild(doseTwo)

                const doseThree = document.createElement('p');
                doseThree.innerText = 'Antal dos 3 injektioner: ' + data.doseThree;
                deSoStatisticContainer.appendChild(doseThree)

                const totalInjections = document.createElement('p');
                totalInjections.innerText = 'Totala antalet injektioner i området: ' + data.totalInjections;
                deSoStatisticContainer.appendChild(totalInjections)

                const ctx = document.getElementById('left-deSo-chart').getContext('2d');
                const chart = JSON.parse(data.jsonChartDose);

                if (leftChart) {
                    leftChart.destroy();
                }

                leftChart = new Chart(ctx, chart);

                if (leftGenderChart) {
                    leftGenderChart.destroy();
                }

                const context = document.getElementById('left-gender-chart').getContext('2d');
                const genderChart = JSON.parse(data.jsonChartGender);

                leftGenderChart = new Chart(context, genderChart);

            })
            .catch((error) => {
                console.error('Error:', error);
            });
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const deSoDropdown = document.getElementById('right-deSo-dropdown');

    deSoDropdown.addEventListener('change', function () {
        let selectedDeSo = this.value;

        fetch('/Home/GetChartFromDeSoCode', {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',

            },
            body: JSON.stringify({ selectedDeSo }),
        })

            .then(response => {
                if (!response.ok) {
                    throw new Error('Inte bra');
                }
                return response.json();
            })
            .then(data => {


                clearDeSoInformation('right-deSo-statistics');
                const deSoStatisticContainer = document.getElementById('right-deSo-statistics');

                const population = document.createElement('p');
                population.innerText = 'Invånare i området: ' + data.population;
                deSoStatisticContainer.appendChild(population);

                const doseOne = document.createElement('p');
                doseOne.innerText = 'Antal dos 1 injektioner: ' + data.doseOne;
                deSoStatisticContainer.appendChild(doseOne)

                const doseTwo = document.createElement('p');
                doseTwo.innerText = 'Antal dos 2 injektioner: ' + data.doseTwo;
                deSoStatisticContainer.appendChild(doseTwo)

                const doseThree = document.createElement('p');
                doseThree.innerText = 'Antal dos 3 injektioner: ' + data.doseThree;
                deSoStatisticContainer.appendChild(doseThree)

                const totalInjections = document.createElement('p');
                totalInjections.innerText = 'Totala antalet injektioner i området: ' + data.totalInjections;
                deSoStatisticContainer.appendChild(totalInjections)



                console.log('Success:', data);

                const ctx = document.getElementById('right-deSo-chart').getContext('2d');
                const chart = JSON.parse(data.jsonChartDose);

                if (rightChart) {
                    rightChart.destroy();
                }

                rightChart = new Chart(ctx, chart);

                if (rightGenderChart) {
                    rightGenderChart.destroy();
                }

                const context = document.getElementById('right-gender-chart').getContext('2d');
                const genderChart = JSON.parse(data.jsonChartGender);

                rightGenderChart = new Chart(context, genderChart);

            })
            .catch((error) => {
                console.error('Error:', error);
            });
    });
});
function clearDeSoInformation(id) {
    const deSoStatisticContainer = document.getElementById(id);

    while (deSoStatisticContainer.firstChild) {
        deSoStatisticContainer.removeChild(deSoStatisticContainer.firstChild);
    }
}

var mymap = L.map('mapid').setView([63.1792, 14.6357], 8); 

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
    maxZoom: 18,
}).addTo(mymap);






