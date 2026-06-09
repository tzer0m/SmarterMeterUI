let allLabels = [];
let allValues = [];
let cumulativeChart;
let consumptionChart;

function initCharts(labels, values) {
    allLabels = labels;
    allValues = values;

    // Cumulative line chart
    cumulativeChart = new Chart(document.getElementById('cumulativeChart').getContext('2d'), {
        type: 'line',
        data: { labels: [], datasets: [{ label: 'kWh', data: [], borderColor: 'rgb(25, 135, 84)', backgroundColor: 'rgba(25, 135, 84, 0.1)', fill: true, tension: 0.3, pointRadius: 0 }] },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                x: { type: 'time', time: { unit: 'day' }, ticks: { maxTicksLimit: 10 } },
                y: { ticks: { callback: val => val.toLocaleString() } }
            },
            plugins: { legend: { display: false } }
        }
    });

    // Consumption bar chart
    consumptionChart = new Chart(document.getElementById('consumptionChart').getContext('2d'), {
        type: 'bar',
        data: { labels: [], datasets: [{ label: 'kWh', data: [], backgroundColor: 'rgba(25, 135, 84, 0.7)', borderColor: 'rgba(25, 135, 84, 1)', borderWidth: 1 }] },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                x: { type: 'time', time: { unit: 'day' }, ticks: { maxTicksLimit: 10 } },
                y: { ticks: { callback: val => val.toLocaleString() } }
            },
            plugins: { legend: { display: false } }
        }
    });

    filterCharts('month', document.querySelector('#rangeButtons .active'));
}

function filterCharts(range, btn) {
    const now = new Date();
    let cutoff;

    if (range === 'today') cutoff = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    else if (range === 'week') cutoff = new Date(now - 7 * 24 * 60 * 60 * 1000);
    else if (range === 'month') cutoff = new Date(now - 30 * 24 * 60 * 60 * 1000);
    else cutoff = null;

    const filtered = allLabels.map((l, i) => ({ l, v: allValues[i] }))
        .filter(d => !cutoff || new Date(d.l) >= cutoff);

    // Update cumulative chart
    cumulativeChart.data.labels = filtered.map(d => d.l);
    cumulativeChart.data.datasets[0].data = filtered.map(d => d.v);
    cumulativeChart.update();

    const consumptionLabels = [];
    const consumptionValues = [];
    const consumptionColors = [];

    for (let i = 1; i < filtered.length; i++) {
        const date = new Date(filtered[i].l);
        const isWeekend = date.getDay() === 0 || date.getDay() === 6;
        consumptionLabels.push(filtered[i].l);
        consumptionValues.push(Math.max(0, +(filtered[i].v - filtered[i - 1].v).toFixed(3)));
        consumptionColors.push(isWeekend ? 'rgba(255, 193, 7, 0.7)' : 'rgba(25, 135, 84, 0.7)');
    }

    consumptionChart.data.labels = consumptionLabels;
    consumptionChart.data.datasets[0].data = consumptionValues;
    consumptionChart.data.datasets[0].backgroundColor = consumptionColors;
    consumptionChart.data.datasets[0].borderColor = consumptionColors.map(c => c.replace('0.7', '1'));
    consumptionChart.update();

    if (btn) {
        document.querySelectorAll('#rangeButtons .btn').forEach(b => b.classList.remove('active'));
        btn.classList.add('active');
    }
}