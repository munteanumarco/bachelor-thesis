import {
  Component,
  ElementRef,
  NgZone,
  OnInit,
  ViewChild,
} from '@angular/core';
import { AuthService } from '../../services/auth.service';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { GeolocationService } from '../../services/geolocation.service';
import { GeocodingService } from '../../services/geocoding.service';
import { CommonModule } from '@angular/common';
import { CreateEmergencyEventRequest } from '../../interfaces/emergency/CreateEmergencyEventRequest';
import { Severity } from '../../interfaces/emergency/Severity';
import { EmergencyType } from '../../interfaces/emergency/EmergencyType';
import { EmergencyEventService } from '../../services/emergency-event.service';

@Component({
  selector: 'app-report-emergency',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './report-emergency.component.html',
  styleUrl: './report-emergency.component.scss',
})
export class ReportEmergencyComponent implements OnInit {
  emergencyForm!: FormGroup;
  loadingLocation = false;
  latitude!: number;
  longitude!: number;

  severityNames: { [key in Severity]: string } = {
    [Severity.Low]: 'Low',
    [Severity.Medium]: 'Medium',
    [Severity.High]: 'High',
    [Severity.Critical]: 'Critical',
  };

  emergencyTypeNames: { [key in EmergencyType]: string } = {
    [EmergencyType.Avalanche]: 'Avalanche',
    [EmergencyType.Drought]: 'Drought',
    [EmergencyType.Earthquake]: 'Earthquake',
    [EmergencyType.Fire]: 'Fire',
    [EmergencyType.Flood]: 'Flood',
    [EmergencyType.Hurricane]: 'Hurricane',
    [EmergencyType.Tornado]: 'Tornado',
    [EmergencyType.Tsunami]: 'Tsunami',
    [EmergencyType.Volcano]: 'Volcano',
  };

  autocomplete: any;
  @ViewChild('locationInput', { static: true }) locationInputRef!: ElementRef;

  constructor(
    private readonly authService: AuthService,
    private formBuilder: FormBuilder,
    private messageService: MessageService,
    private router: Router,
    private geolocationService: GeolocationService,
    private geocodingService: GeocodingService,
    private ngZone: NgZone,
    private readonly emergencyEventService: EmergencyEventService
  ) {}

  ngOnInit(): void {
    this.emergencyForm = this.formBuilder.group({
      type: ['', Validators.required],
      description: [''],
      severity: ['', Validators.required],
      location: ['', Validators.required],
    });
    this.initAutocomplete();
  }

  getLocation(): void {
    this.loadingLocation = true;
    this.geolocationService
      .getCurrentLocation()
      .then((position) => {
        const point: google.maps.LatLngLiteral = {
          lat: position.coords.latitude,
          lng: position.coords.longitude,
        };
        this.latitude = position.coords.latitude;
        this.longitude = position.coords.longitude;

        this.geocodingService.geocodeLatLng(point).then((response) => {
          const address = response.results[0].formatted_address;
          this.emergencyForm.controls['location'].setValue(address);
          this.loadingLocation = false;
        });
      })
      .catch((error) => {
        this.showError('Could not get current location');
      });
  }

  initAutocomplete(): void {
    this.autocomplete = new google.maps.places.Autocomplete(
      this.locationInputRef.nativeElement
    );
    this.autocomplete.addListener('place_changed', () =>
      this.ngZone.run(() => this.onLocationSelected())
    );
  }

  onLocationSelected(): void {
    const place = this.autocomplete.getPlace();
    if (place.geometry) {
      this.emergencyForm.controls['location'].setValue(place.formatted_address);
      this.latitude = place.geometry.location.lat();
      this.longitude = place.geometry.location.lng();
    }
  }

  reportEvent(): void {
    const data: CreateEmergencyEventRequest = {
      description: this.emergencyForm.value.description,
      location: this.emergencyForm.value.location,
      latitude: this.latitude,
      longitude: this.longitude,
      severity: parseInt(this.emergencyForm.value.severity),
      type: parseInt(this.emergencyForm.value.type),
    };

    this.emergencyEventService.reportEmergency(data).subscribe({
      next: () => {
        this.showSuccess();
        this.router.navigate(['/']);
      },
      error: () => {
        this.showError("Couldn't report emergency event");
      },
    });
  }

  showSuccess(): void {
    this.messageService.add({
      key: 'bc',
      severity: 'success',
      summary: 'Success',
      detail: 'Emergency event reported successfully',
    });
  }

  showError(error: string): void {
    this.messageService.add({
      key: 'bc',
      severity: 'error',
      summary: 'Error',
      detail: error,
    });
  }
}
