import { Component, OnInit } from '@angular/core';
import { OwnerService } from 'src/app/Services/owner.service';
import { Report } from 'src/app/Models/report.model';

@Component({
  selector: 'app-report',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {
  report: Report | null = null;

  constructor(private ownerService: OwnerService) {}

  ngOnInit(): void {
    this.loadReport();
  }

  loadReport(): void {
    this.ownerService.getReport().subscribe(
      (data: Report) => {
        this.report = data;
      },
      error => {
        console.error('Error fetching report:', error);
      }
    );
  }
}
