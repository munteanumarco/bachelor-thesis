import { Component, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-report-emergency',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './report-emergency.component.html',
  styleUrl: './report-emergency.component.scss',
})
export class ReportEmergencyComponent implements OnInit {
  emergencyForm!: FormGroup;
  loadingLocation = false;
  latitude!: number;
  longitude!: number;

  constructor(
    private readonly authService: AuthService,
    private formBuilder: FormBuilder,
    private messageService: MessageService,
    private router: Router,
    private geolocationService: GeolocationService,
    private geocodingService: GeocodingService
  ) {}

  ngOnInit(): void {
    this.emergencyForm = this.formBuilder.group({
      type: ['', Validators.required],
      description: [''],
      severity: ['', Validators.required],
      location: ['', Validators.required],
    });
  }

  onSubmit(): void {}

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
        console.error('Error getting location', error);
      });
  }
}
