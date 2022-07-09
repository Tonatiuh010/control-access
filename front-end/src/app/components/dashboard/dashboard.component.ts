import { Component, HostListener, OnInit } from '@angular/core';
import {  EChartsOption } from 'echarts';



@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  chartOption: EChartsOption = {
    devicePixelRatio: 100,
    title: {
      text: 'Daily Attendence',
      textStyle: {
        fontFamily: 'sans-serif'
      },
    },
    tooltip: {
      trigger: 'item'
    },
    legend: {
      orient: 'horizontal',
    },
    series: [
      {
        name: 'Time Of Arrival',
        type: 'pie',
        radius: '50%',
        data: [
          { value: 20, name: 'Absent', itemStyle: {color: '#ee6666'}},
          { value: 66, name: 'On-Time', itemStyle: {color: '#91cc75'}},
          { value: 12, name: 'Tardies', itemStyle: {color: '#fac858'} }
        ],
        emphasis: {
          itemStyle: {
            shadowBlur: 10,
            shadowOffsetX: 0,
            shadowColor: 'rgba(0, 0, 0, 0.5)'
          }
        }
      }
    ]
  };


  chartOption2: EChartsOption = {
    title: {
      text: 'Monthly Total Attendance'
    },
    legend: {},
    tooltip: {},
    dataset: {
      source: [
        ['Month', 'June', 'July', 'August'],
        ['Security', 43, 85, 93],
        ['RH', 43, 23, 25],
        ['Production', 12, 45, 83],
        ['Sales ', 72, 53, 39],
        ['Warehouse', 23, 23, 32]
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
      text: 'Weekly Attendance by Department'
    },
    tooltip: {
      trigger: 'axis',
      axisPointer: {
        // Use axis to trigger tooltip
        type: 'shadow' // 'shadow' as default; can also be 'line' or 'shadow'
      }
    },
    legend: {},
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
