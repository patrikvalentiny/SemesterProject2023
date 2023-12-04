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
import {WeightService} from "../../services/weight.service";
import {UserDetailsService} from "../../services/user-details.service";

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
    private readonly userService: UserDetailsService = inject(UserDetailsService);

    constructor() {

        const maxWeight = 140;
        const minWeight = 0;
        const maxBmi = 40;
        const minBmi = 0;
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
                        selection: true,
                        zoom: true,
                        zoomin: true,
                        zoomout: true,
                        pan: true,
                    }
                },
                zoom: {
                    enabled: true,
                    type: "xy"

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
                        yAxisIndex: 0,
                        y: 0,
                        y2: 0,
                        borderColor: "#00dbe3",
                        fillColor: "#00dbe3",
                        opacity: 1,
                        label: {
                            position: "left",
                            textAnchor: "start",
                            offsetX: 10,
                            borderColor: "#333",
                            style: {
                                fontSize: "10px",
                                color: "#333",
                                background: "#00dbe3"
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
                        yAxisIndex: 1,
                        y: 18.49,
                        y2: 0,
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
        await this.userService.getProfile();
        const height = this.userService.user!.height / 100;
        const targetWeight = this.userService.user!.targetWeight;
        const targetDate = this.userService.user!.targetDate;

        const weights: number[] = this.weightService.weights.map(weight => weight.weight).reverse();
        const dates: string[] = this.weightService.weights.map(weight => weight.date.toString()).reverse();
        dates.push(targetDate.toString());
        const maxWeight = Math.max(...weights) + 2;
        const minWeight = targetWeight - 2;
        this.chartOptions.yaxis![0].max = maxWeight;
        this.chartOptions.yaxis![0].min = minWeight;

        const maxBmi = maxWeight / (height * height);
        const minBmi = minWeight / (height * height);
        this.chartOptions.yaxis![1].max = maxBmi;
        this.chartOptions.yaxis![1].min = minBmi;

        this.chartOptions.series = [
            {
                name: "Weight",
                data: weights
            },
            {
                name: "BMI",
                data: weights.map(weight => +((weight / (height * height)).toFixed(2)))
            }
        ];
        this.chartOptions.xaxis = {
            type: "datetime",
            labels: {
                format: "dd/MM/yy",
              datetimeUTC: false,

            },

            categories: dates
        };

      this.chartOptions.annotations!.yaxis![0].y= targetWeight;
      this.chartOptions.annotations!.yaxis![0].y2= targetWeight - 0.2;
      this.chartOptions.annotations!.yaxis![1].y2 = maxBmi;

    }

}
