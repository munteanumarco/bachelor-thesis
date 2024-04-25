import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmergencyMapComponent } from './emergency-map.component';

describe('EmergencyMapComponent', () => {
  let component: EmergencyMapComponent;
  let fixture: ComponentFixture<EmergencyMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmergencyMapComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EmergencyMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
