import { Component, HostListener, OnInit } from '@angular/core';
import {  EChartsOption } from 'echarts';
import { EmployeeService } from 'src/app/services/employee-service.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  hourlyDataSet!: any[];
  chartOption2!: EChartsOption;

  chartOption3: EChartsOption = {
    title: {
      text: 'Weekly Attendance'
    },
    tooltip: {
      trigger: 'axis',
      axisPointer: {
        // Use axis to trigger tooltip
        type: 'shadow' // 'shadow' as default; can also be 'line' or 'shadow'
      }
    },
    legend: {
      left: 'right'
    },
    grid: {
      left: '3%',
      right: '4%',
      bottom: '3%',
      containLabel: true
    },
    xAxis: {
      type: 'value'
    },
    yAxis: {
      type: 'category',
      data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
    },
    series: [
      {
        name: 'Engineering',
        type: 'bar',
        stack: 'total',
        label: {
          show: true
        },
        emphasis: {
          focus: 'series'
        },
        data: [12, 13, 10, 13, 9, 23, 21]
      },
      {
        name: 'Human Resources',
        type: 'bar',
        stack: 'total',
        label: {
          show: true
        },
        emphasis: {
          focus: 'series'
        },
        data: [22, 18, 19, 23, 29, 33, 31]
      },
      {
        name: 'Production',
        type: 'bar',
        stack: 'total',
        label: {
          show: true
        },
        emphasis: {
          focus: 'series'
        },
        data: [15, 22, 21, 15, 19, 33, 41]
      },
      {
        name: 'Design',
        type: 'bar',
        stack: 'total',
        label: {
          show: true
        },
        emphasis: {
          focus: 'series'
        },
        data: [20, 32, 10, 34, 29, 33, 20]
      }
    ]
  };
  showSpinner: boolean = false;
  cardArray: any;

  constructor(private employeeService: EmployeeService) { }

  ngOnInit(): void {
    this.initializeDashboard()
  }

  initializeDashboard(){
    this.showSpinner = true;
    this.employeeService.getDashboardInfo().subscribe(data =>{
      this.hourlyDataSet = this.buildHourlyDataset(data.data);
      this.cardArray = data.data2;
      this.buildHourlyChart();
      this.showSpinner = false;
    })
  }

  buildHourlyDataset(data: any): any[]{
    let dataSet: any[] = []
    let temporaryArray: any[] = []
    data.forEach((element: any) => {
      temporaryArray.unshift(element.sets[0].departament.name)
      element.sets.forEach((element: any) => {
        temporaryArray.push(element.checks)
      });
      dataSet.push(temporaryArray)
      temporaryArray = []
    });
    return dataSet;
  }

  buildHourlyChart(){
    this.chartOption2 = {
      width: '83%',
      title: {
        text: 'Hourly Attendance'
      },
      legend: {
        left: 'right'
      },
      tooltip: {},
      dataset: {
        source: [
          ['','6:00 AM - 10:00 AM', '10:00 AM - 4:00 PM', '4:00 PM - 8:00 PM'],
          this.hourlyDataSet[0],
          this.hourlyDataSet[1],
          this.hourlyDataSet[2],
          this.hourlyDataSet[3],
        ]
      },
      xAxis: { type: 'category' },
      yAxis: {},
      // Declare several bar series, each will be mapped
      // to a column of dataset.source by default.
      series: [{ type: 'bar' }, { type: 'bar' }, { type: 'bar' }],

    };
  }

}
