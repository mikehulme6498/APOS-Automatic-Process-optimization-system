var gaugeOptions = {
    chart: {
        type: 'solidgauge',
    },
    title: null,

    pane: {
        center: ['50%', '50%'],
        size: '100%',
        startAngle: 0,
        endAngle: 360,
        background: {
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#EEE',
            innerRadius: '60%',
            outerRadius: '100%',
            shape: 'circle'
        }
    },

    exporting: {
        enabled: false
    },

    tooltip: {
        enabled: false
    },

    // the value axis
    yAxis: {
        stops: [
            [0.70, '#DF5353'],  // red
            [0.84, '#DDDF0D'], // yellow
            [0.85, '#55BF3B'], // green
        ],
        lineWidth: 0,
        tickWidth: 0,
        minorTickInterval: null,
        tickAmount: 2,
        labels: {
            enabled: false,
        }
    },
    plotOptions: {
        solidgauge: {
            dataLabels: {
                borderWidth: 0,
                useHTML: true
                //enabled: false
            }
        }
    }
};