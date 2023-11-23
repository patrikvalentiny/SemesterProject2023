import {Component, inject, OnInit, ViewChild} from '@angular/core';
import {CommonModule} from '@angular/common';
import {
    ApexAnnotations,
    ApexAxisChartSeries,
    ApexChart,
    ApexDataLabels,
    ApexStroke,
    ApexTheme,
    ApexTitleSubtitle,
    ApexXAxis,
    ApexYAxis,
    ChartComponent,
    NgApexchartsModule
} from "ng-apexcharts";
import {WeightService} from "../../weight-controls/weight.service";

export type ChartOptions = {
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
};

@Component({
    selector: 'app-weight-line-chart',
    standalone: true,
    imports: [CommonModule, NgApexchartsModule],
    templateUrl: './weight-line-chart.component.html',
    styleUrl: './weight-line-chart.component.css'
})
export class WeightLineChartComponent implements OnInit {
    @ViewChild("chart") chart!: ChartComponent;
    public chartOptions: Partial<ChartOptions>;
    private readonly weightService: WeightService = inject(WeightService);
    private height = 1.87;

    constructor() {
        const targetWeight = 87.3;

        const maxWeight = 140;
        const minWeight = 80;
        const maxBmi = maxWeight / (this.height * this.height);
        const minBmi = minWeight / (this.height * this.height);
        this.chartOptions = {
            series: [
                {
                    name: "Weight",
                    data: [0]
                }
            ],
            chart: {
                height: 350,
                type: "line",
                background: "rgba(0,0,0,0)",
                toolbar: {
                    show: true,
                    offsetX: 0,
                    offsetY: 0,
                    tools: {
                        download: true,
                        selection: false,
                        zoom: true,
                        zoomin: false,
                        zoomout: false,
                        pan: false,
                    }
                },
                zoom: {
                    enabled: false,
                    type: "x"

                }
            },
            yaxis: [
                {
                    seriesName: "Weight",
                    title: {text: "Weight (kg)"},
                    max: maxWeight,
                    min: minWeight,
                },
                {
                    seriesName: "BMI",
                    opposite: true,
                    title: {text: "BMI"},
                    max: maxBmi,
                    min: minBmi,
                }
            ],
            dataLabels: {
                enabled: false
            },
            theme: {
                mode: "dark",
                palette: "palette10"
            },
            title: {
                text: "Your weight history"
            },
            stroke: {
                show: true,
                curve: "straight"
            },
            colors: ['#008FFB'],
            annotations: {
                yaxis: [
                    {
                        y: targetWeight,
                        y2: targetWeight + 0.1,
                        borderColor: "#00E396",
                        fillColor: "#00E396",
                        opacity: 1,
                        label: {
                            position: "left",
                            textAnchor: "start",
                            offsetX: 10,
                            borderColor: "#333",
                            style: {
                                fontSize: "10px",
                                color: "#333",
                                background: "#00E396"
                            },
                            text: "Target weight"
                        }
                    },
                    {
                        yAxisIndex: 1,
                        y: 30,
                        y2: maxBmi,
                        borderColor: "#000",
                        fillColor: "#fc4949",
                        opacity: 0.1,
                        label: {
                            borderColor: "#333",
                            style: {
                                fontSize: "10px",
                                color: "#333",
                                background: "#fc4949"
                            },
                            text: "Obese"
                        }
                    },
                    {
                        yAxisIndex: 1,
                        y: 29.99,
                        y2: 25,
                        borderColor: "#000",
                        fillColor: "#FEB019",
                        opacity: 0.1,
                        label: {
                            borderColor: "#333",
                            style: {
                                fontSize: "10px",
                                color: "#333",
                                background: "#FEB019"
                            },
                            text: "Overweight"
                        }
                    },
                    {
                        yAxisIndex: 1,
                        y: 24.99,
                        y2: 18.5,
                        borderColor: "#000",
                        fillColor: "#00E396",
                        opacity: 0.1,
                        label: {
                            borderColor: "#333",
                            style: {
                                fontSize: "10px",
                                color: "#333",
                                background: "#00E396"
                            },
                            text: "Normal weight"
                        }
                    },
                    {
                        y: this.height * this.height * 18.49,
                        borderColor: "#000",
                        fillColor: "#008FFB",
                        opacity: 0.1,
                        label: {
                            borderColor: "#333",
                            style: {
                                fontSize: "10px",
                                color: "#333",
                                background: "#008FFB"
                            },
                            text: "Underweight"
                        }
                    }
                ]
            }
        };
    }

    async ngOnInit() {
        await this.weightService.getWeights();
        const weights: number[] = this.weightService.weights.map(weight => weight.weight).reverse();
        const dates: string[] = this.weightService.weights.map(weight => new Date(weight.date).toLocaleString()).reverse();
        const maxWeight = Math.max(...weights) + 5;
        this.chartOptions.yaxis![0].max = maxWeight;
        this.chartOptions.yaxis![1].max = maxWeight / (this.height * this.height);
        this.chartOptions.series = [
            {
                name: "Weight",
                data: weights
            },
            {
                name: "BMI",
                data: weights.map(weight => +((weight / (1.87 * 1.87)).toFixed(2)))
            }
        ];
        this.chartOptions.xaxis = {
            type: "datetime",
            categories: dates
        };

    }

}
