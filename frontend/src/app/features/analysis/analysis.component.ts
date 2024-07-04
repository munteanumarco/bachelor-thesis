import { Component, OnInit } from '@angular/core';
import { EmergencyType } from '../../interfaces/emergency/EmergencyType';
import { getEmergencyTypeName } from '../../interfaces/emergency/EmergencyType';
import { AnalysisService } from '../../services/analysis.service';
import { LandCoverAnalysisStatus } from '../../interfaces/analysis/LandcoverAnalysisStatus';
import { getLandCoverAnalysisStatusName } from '../../interfaces/analysis/LandcoverAnalysisStatus';
import { EmergencyDetailsDto } from '../../interfaces/emergency/EmergencyDetailsDto';
import { EmergencyEventService } from '../../services/emergency-event.service';
import { ActivatedRoute } from '@angular/router';
import { LandCoverAnalysisDto } from '../../interfaces/analysis/LandcoverAnalysisDto';
import { forkJoin } from 'rxjs';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-analysis',
  standalone: true,
  imports: [],
  templateUrl: './analysis.component.html',
  styleUrl: './analysis.component.scss',
})
export class AnalysisComponent implements OnInit {
  event!: EmergencyDetailsDto;
  analysis!: LandCoverAnalysisDto;
  completedStatus = LandCoverAnalysisStatus.Completed;
  notTriggered = LandCoverAnalysisStatus.NotTriggered;

  constructor(
    private analysisService: AnalysisService,
    private emergencyEventService: EmergencyEventService,
    private route: ActivatedRoute,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const eventId = params['eventId'];
      this.fetchAnalysisDetails(eventId);
      this.fetchEventDetails(eventId);
    });
  }

  getEmergencyTypeName(type: EmergencyType): string {
    return getEmergencyTypeName(type);
  }

  getLandCoverAnalysisStatusName(status: LandCoverAnalysisStatus): string {
    console.log('status inside function:', status);
    if (!status) return '';
    return getLandCoverAnalysisStatusName(status);
  }

  fetchEventDetails(eventId: string): void {
    this.emergencyEventService
      .getEmergencyEventDetails(eventId)
      .subscribe((response) => {
        this.event = response.details;
      });
  }

  fetchAnalysisDetails(eventId: string): void {
    this.analysisService.getAnalysis(eventId).subscribe((response) => {
      this.analysis = response;
    });
  }

  startAnalysis(): void {
    this.analysisService
      .startAnalysis(this.event.id, this.event.latitude, this.event.longitude)
      .subscribe({
        next: (response) => {
          this.showSuccess(response.message);
          this.fetchAnalysisDetails(this.event.id);
        },
        error: (error) => {
          this.showErrorMessage(error);
        },
      });
  }

  showSuccess(message: string): void {
    this.messageService.add({
      key: 'bc',
      severity: 'success',
      summary: 'Success',
      detail: message,
    });
  }

  showErrorMessage(error: string): void {
    this.messageService.add({
      key: 'bc',
      severity: 'error',
      summary: 'Login Failed',
      detail: error,
    });
  }
}
