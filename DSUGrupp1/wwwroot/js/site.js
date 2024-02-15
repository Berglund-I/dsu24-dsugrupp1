// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let leftChart;
let rightChart;
let leftGenderChart;
let rightGenderChart;
let rightFilterChart;
let leftOverTimeChart; 
let rightOverTimeChart;

let todaysDate = new Date();
let dd = todaysDate.getDate();
let mm = todaysDate.getMonth() + 1;
if (mm < 10) { mm = '0' + mm; }
let yyyy = todaysDate.getFullYear();
let newDate = yyyy + "-" + mm + "-" + dd;

let endDate = document.getElementById('end-date');
endDate = newDate;


document.addEventListener('DOMContentLoaded', function () {
    filterButton = document.getElementById('confirm-filters');
    filterButton.addEventListener('click', function () {

        const deSoCode = document.getElementById('filter-deSo-dropdown').value;
        const gender = document.getElementById('gender-drop-down').value;
        const minAge = document.getElementById('input-left').value;
        const maxAge = document.getElementById('input-right').value;
        const batchNumber = document.getElementById('batch-number-dropdown').value;
        const vaccineType = document.getElementById('vaccine-type-dropdown').value;
        let vaccineCentral = document.getElementById('vaccine-central-dropdown').value;
        if (vaccineCentral == "") {
            vaccineCentral = 0;
        }
        const doseCount = document.getElementById('dose-dropdown').value;
        const startDate = document.getElementById('start-date').value;
        endDate = document.getElementById('end-date').value;

        const vaccineData = {
            deSoCode: deSoCode,
            batchNumber: batchNumber,
            gender: gender,
            minAge: minAge,
            maxAge: maxAge,
            siteId: vaccineCentral,
            numberOfDoses: doseCount,
            typeOfVaccine: vaccineType,
            startDate: startDate,
            endDate: endDate,
        };
        console.log(vaccineData);

        fetch('/Home/GetChartFromFilteredOptions', {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',

            },
            body: JSON.stringify(vaccineData),
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Inte bra');
                }
                return response.json();
            })
            .then(data => {

                if (rightFilterChart) {
                    rightFilterChart.destroy();
                }

                const contextTest = document.getElementById('right-filter-chart').getContext('2d');
                rightFilterChart = new Chart(contextTest, data);

            })
            .catch((error) => {
                console.error('Error:', error);
            });
        resetSliders();
    });
});


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
                const leftGenderChartParagraph = document.getElementById("gender-paragraf-left");
                leftGenderChartParagraph.textContent = "Cirkeldiagram som presenterar vaccinationsgraden mellan kvinnor och män i antal personer som är vaccinerade..";


                if (leftOverTimeChart) {
                    leftOverTimeChart.destroy();
                }

                const ctext = document.getElementById('left-over-time-chart').getContext('2d');
                const overTimeChart = JSON.parse(data.jsonChartVaccinationOverTime);

                leftOverTimeChart = new Chart(ctext, overTimeChart);
                const leftOverTimeChartParagraph = document.getElementById("vaccination-over-time-paragraf-left");
                leftOverTimeChartParagraph.textContent = "Ett linjegram som visar antal vaccinerade per vecka under åren 2020-2023.";

                

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
                const rightGenderChartParagraph = document.getElementById("gender-paragraf-right");
                rightGenderChartParagraph.textContent = "Cirkeldiagram som presenterar vaccinationsgraden mellan kvinnor och män i antal personer som är vaccinerade.";
                

                if (rightOverTimeChart) {
                    rightOverTimeChart.destroy();
                }

                const ctext = document.getElementById('right-over-time-chart').getContext('2d');
                const overTimeChart = JSON.parse(data.jsonChartVaccinationOverTime);

                rightOverTimeChart = new Chart(ctext, overTimeChart);
                const rightOverTimeChartParagraph = document.getElementById("vaccination-over-time-paragraf-right");
                rightOverTimeChartParagraph.textContent = "Ett linjegram som visar antal vaccinerade per vecka under åren 2020-2023.";

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


var inputLeft = document.getElementById("input-left");
var inputRight = document.getElementById("input-right");

var thumbLeft = document.querySelector(".slider > .thumb.left");
var thumbRight = document.querySelector(".slider > .thumb.right");
var range = document.querySelector(".slider > .range");

var leftValueDisplay = document.getElementById("left-value");
var rightValueDisplay = document.getElementById("right-value");

var confirmButton = document.getElementById("confirm-button");

function setLeftValue() {
    var sliderInputL = inputLeft,
        min = parseInt(sliderInputL.min),
        max = parseInt(sliderInputL.max);

    sliderInputL.value = Math.min(parseInt(sliderInputL.value), parseInt(inputRight.value) - 5);

    var percent = ((sliderInputL.value - min) / (max - min)) * 100;

    thumbLeft.style.left = percent + "%";
    range.style.left = percent + "%";

    leftValueDisplay.innerText = sliderInputL.value;
    leftValueDisplay.style.left = thumbLeft.style.left;
}

function setRightValue() {
    var sliderInputR = inputRight,
        min = parseInt(sliderInputR.min),
        max = parseInt(sliderInputR.max);

    sliderInputR.value = Math.max(parseInt(sliderInputR.value), parseInt(inputLeft.value) + 5);

    var percent = ((sliderInputR.value - min) / (max - min)) * 100;

    thumbRight.style.right = (100 - percent) + "%";
    range.style.right = (100 - percent) + "%";

    rightValueDisplay.innerText = sliderInputR.value;
    rightValueDisplay.style.right = thumbRight.style.right;
}

setLeftValue();
setRightValue();

inputLeft.addEventListener("input", setLeftValue);
inputRight.addEventListener("input", setRightValue);

inputLeft.addEventListener("mouseover", function () {
    thumbLeft.classList.add("hover");
});
inputLeft.addEventListener("mouseout", function () {
    thumbLeft.classList.remove("hover");
});
inputLeft.addEventListener("mousedown", function () {
    thumbLeft.classList.add("active");
});
inputLeft.addEventListener("mouseup", function () {
    thumbLeft.classList.remove("active");
});

inputRight.addEventListener("mouseover", function () {
    thumbRight.classList.add("hover");
});
inputRight.addEventListener("mouseout", function () {
    thumbRight.classList.remove("hover");
});
inputRight.addEventListener("mousedown", function () {
    thumbRight.classList.add("active");
});
inputRight.addEventListener("mouseup", function () {
    thumbRight.classList.remove("active");
});

confirmButton.addEventListener("click", function () {
    sendSliderValues();
    inputLeft.disabled = true;
    inputRight.disabled = true;
});


var ageChart = {};
function sendSliderValues() {
    var leftValue = document.getElementById("input-left").value;
    var rightValue = document.getElementById("input-right").value;

    fetch('/Home/CreateChartBasedOnSelectedMinAgeAndMaxAge', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ leftValue, rightValue }),
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Förfrågan misslyckades');
            }
            return response.json();
        })
        .then(data => {
            var newChartData = JSON.parse(data.jsonChart);

            var oldCanvas = document.getElementById('chart7');
            var newCanvas = document.createElement('canvas');
            newCanvas.id = oldCanvas.id;
            oldCanvas.parentNode.replaceChild(newCanvas, oldCanvas);

            var ctx = newCanvas.getContext('2d');

            ageChart['chart7'] = new Chart(ctx, newChartData);

            setLeftValue();
            setRightValue();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}



function resetSliders() {
    document.getElementById("input-left").value = 16;
    document.getElementById("input-right").value = 85;

    setLeftValue();
    setRightValue();
}


var resetButton = document.getElementById("reset-button");
resetButton.style.display = "none";

var confirmButton = document.getElementById("confirm-button");

confirmButton.addEventListener("click", function () {
    sendSliderValues();
    inputLeft.disabled = false;
    inputRight.disabled = false;
    resetSliders();

    resetButton.style.display = "inline-block";


});



function resetChart() {

    fetch('/Home/ResetChartToShowTheWholePopulation', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Förfrågan misslyckades');
            }
            return response.json();
            })
            
        .then(data => {
            var newChartData = JSON.parse(data.jsonChart);

            var oldCanvas = document.getElementById('chart7');
            var newCanvas = document.createElement('canvas');
            newCanvas.id = oldCanvas.id;
            oldCanvas.parentNode.replaceChild(newCanvas, oldCanvas);

            var ctx = newCanvas.getContext('2d');

            ageChart['chart7'] = new Chart(ctx, newChartData);

            setLeftValue();
            setRightValue();
        })
        .catch((error) => {
            console.error('Error:', error);
        });

}

var resetButton = document.getElementById("reset-button");

resetButton.addEventListener("click", function () {
    resetChart();
    inputLeft.disabled = false;
    inputRight.disabled = false;
    resetSliders();

    resetButton.style.display = "none";
});


