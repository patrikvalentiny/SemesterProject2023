import {
  ApexAnnotations,
  ApexAxisChartSeries,
  ApexChart, ApexDataLabels, ApexLegend, ApexMarkers, ApexNonAxisChartSeries, ApexPlotOptions,
  ApexStroke,
  ApexTheme,
  ApexTitleSubtitle, ApexTooltip,
  ApexXAxis,
  ApexYAxis
} from "ng-apexcharts";

export type AxisChartOptions = {
  series: ApexAxisChartSeries;
  theme: ApexTheme;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis[];
  title: ApexTitleSubtitle;
  stroke: ApexStroke;
  dataLabels: ApexDataLabels;
  annotations: ApexAnnotations;
  colors: string[];
  tooltip: ApexTooltip;
  markers: ApexMarkers;
  legend: ApexLegend;
};

export type NonAxisChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  labels: string[];
  plotOptions: ApexPlotOptions;
  colors: string[];
  theme: ApexTheme;
  dataLabels: ApexDataLabels;
  tooltip: ApexTooltip;
  legend: ApexLegend;
};

export type ChartOptions = AxisChartOptions | NonAxisChartOptions;

export const defaultAxisChartOptions: Partial<AxisChartOptions>= {
  colors: ["#dca54c"],
  stroke: {
    show: true,
    curve: "smooth",
  },
  theme: {
    mode: "dark",
    palette: "palette10"
  },
  dataLabels: {
    enabled: false
  },
}

