import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserService } from '../services/UserService';
import { Chart, ChartConfiguration, ChartType, registerables } from 'chart.js';
import { CommonModule } from '@angular/common';

Chart.register(...registerables);

@Component({
  selector: 'app-home',
  imports: [CommonModule],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit{
  @ViewChild('genderChartCanvas') genderChartRef! : ElementRef;
  @ViewChild('roleChartCanvas') roleChartRef! : ElementRef;

  users: any[] = [];
 
  constructor(private userService: UserService) {}
 
  ngOnInit(): void {
    this.userService.users$.subscribe(users => {
      this.users = users;
      this.renderGenderChart(users);
      this.renderRoleChart(users);
    });
 
    this.userService.fetchData();
  }
 
  renderGenderChart(users: any[]) {
    const males = users.filter(u => u.gender === 'male').length;
    const females = users.filter(u => u.gender === 'female').length;
 
    new Chart(this.genderChartRef.nativeElement.getContext('2d'), {
      type: 'pie',
      data: {
        labels: ['Male', 'Female'],
        datasets: [{
          data: [males, females],
          backgroundColor: ['#36A2EB', '#FF6384']
        }]
      }
    });
  }
 
  renderRoleChart(users: any[]) {
    const roleCounts: { [key: string]: number } = {};
    users.forEach(user => {
      const role = user.company?.title || 'Unknown';
      roleCounts[role] = (roleCounts[role] || 0) + 1;
    });
 
    new Chart(this.roleChartRef.nativeElement.getContext('2d'), {
      type: 'bar',
      data: {
        labels: Object.keys(roleCounts),
        datasets: [{
          label: 'Users per Role',
          data: Object.values(roleCounts),
          backgroundColor: '#007acc'
        }]
      },
      options: {
        responsive: true,
        scales: {
          y: {
            beginAtZero: true
          }
        }
      }
    });
  }
}
