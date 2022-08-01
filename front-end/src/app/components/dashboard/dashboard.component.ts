import { Component, HostListener, OnInit } from '@angular/core';
import {  EChartsOption } from 'echarts';



@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  chartOption2: EChartsOption = {
    title: {
      text: 'Hourly Attendance'
    },
    legend: {
      left: 'right'
    },
    tooltip: {},
    dataset: {
      source: [
        ['7:00 AM', '10:00 AM', '2:00 PM', '5:00 PM'],
        ['Security', 43, 85, 93, 10],
        ['RH', 43, 23, 25, 10],
        ['Production', 12, 45, 83, 10],
        ['Sales ', 72, 53, 39, 10],
        ['Warehouse', 23, 23, 32, 10]
      ]
    },
    xAxis: { type: 'category' },
    yAxis: {},
    // Declare several bar series, each will be mapped
    // to a column of dataset.source by default.
    series: [{ type: 'bar' }, { type: 'bar' }, { type: 'bar' }]
  };

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
        name: 'Security',
        type: 'bar',
        stack: 'total',
        label: {
          show: true
        },
        emphasis: {
          focus: 'series'
        },
        data: [32, 30, 30, 33, 39, 33, 32]
      },
      {
        name: 'RH',
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
        name: 'Production',
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
        name: 'Sales',
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
        name: 'Warehouse',
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

  constructor() { }

  ngOnInit(): void {
  }

}
