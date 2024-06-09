import { Component, OnInit } from '@angular/core';
import { EmergencyEventDto } from '../../interfaces/emergency/EmergencyEventDto';
import { EmergencyType } from '../../interfaces/emergency/EmergencyType';
import { Severity } from '../../interfaces/emergency/Severity';
import { Status } from '../../interfaces/emergency/Status';
import { getEmergencyTypeName } from '../../interfaces/emergency/EmergencyType';
import { AnalysisService } from '../../services/analysis.service';
import { LandCoverAnalysisStatus } from '../../interfaces/analysis/LandcoverAnalysisStatus';
import { getLandCoverAnalysisStatusName } from '../../interfaces/analysis/LandcoverAnalysisStatus';

@Component({
  selector: 'app-analysis',
  standalone: true,
  imports: [],
  templateUrl: './analysis.component.html',
  styleUrl: './analysis.component.scss',
})
export class AnalysisComponent implements OnInit {
  event: EmergencyEventDto = {
    id: '4',
    description: 'Fire in a building',
    location: 'Zimbrului Street, Iasi, Romania',
    latitude: 47.1584549,
    longitude: 27.6014418,
    severity: Severity.CRITICAL,
    status: Status.NEW,
    type: EmergencyType.FIRE,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'John Doe',
  };
  analysisStatus: LandCoverAnalysisStatus | undefined;
  completedStatus = LandCoverAnalysisStatus.Completed;

  constructor(private analysisService: AnalysisService) {}

  ngOnInit(): void {
    this.analysisService
      .getUp()
      .subscribe(() => console.log('Analysis service is up'));
  }

  getEmergencyTypeName(type: EmergencyType): string {
    return getEmergencyTypeName(type);
  }

  getLandCoverAnalysisStatusName(status: LandCoverAnalysisStatus): string {
    return getLandCoverAnalysisStatusName(status);
  }

  startAnalysis() {
    this.analysisStatus = LandCoverAnalysisStatus.InProgress;
    setTimeout(() => {
      this.analysisStatus = LandCoverAnalysisStatus.Completed;
    }, 10000);
  }
}
