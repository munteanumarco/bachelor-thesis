import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportEmergencyComponent } from './report-emergency.component';

describe('ReportEmergencyComponent', () => {
  let component: ReportEmergencyComponent;
  let fixture: ComponentFixture<ReportEmergencyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReportEmergencyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ReportEmergencyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
