flatpickr("#datepicker-range", {
  mode: "range",
  dateFormat: "d M, Y"
}), flatpickr("#date-field", {
  dateFormat: "d M, Y"
});

function getChartColorsArray(e) {
  if (null !== document.getElementById(e)) {
    var e = document.getElementById(e).getAttribute("data-colors");
    return (e = JSON.parse(e)).map(function (e) {
      var t = e.replace(" ", "");
      if (-1 === t.indexOf(",")) {
        var r = getComputedStyle(document.documentElement).getPropertyValue(t);
        return r || t
      }
      e = e.split(",");
      return 2 != e.length ? t : "rgba(" + getComputedStyle(document.documentElement).getPropertyValue(e[0]) + "," + e[1] + ")"
    })
  }
}

var isApexSeriesData = {},
  isApexSeries = document.querySelectorAll("[data-chart-series]");
isApexSeries.forEach(function (e) {
  var t = e.attributes;
  t["data-chart-series"] && (isApexSeriesData.series = t["data-chart-series"].value.toString(), e = getChartColorsArray(t.id.value.toString()), e = {
    series: [isApexSeriesData.series],
    chart: {
      type: "radialBar",
      width: 36,
      height: 36,
      sparkline: {
        enabled: !0
      }
    },
    dataLabels: {
      enabled: !1
    },
    plotOptions: {
      radialBar: {
        hollow: {
          margin: 0,
          size: "50%"
        },
        track: {
          margin: 1
        },
        dataLabels: {
          show: !1
        }
      }
    },
    colors: e
  }, new ApexCharts(document.querySelector("#" + t.id.value.toString()), e).render())
});

