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

@Component({
  selector: 'app-report-emergency',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './report-emergency.component.html',
  styleUrl: './report-emergency.component.scss',
})
export class ReportEmergencyComponent implements OnInit {
  emergencyForm!: FormGroup;

  constructor(
    private readonly authService: AuthService,
    private formBuilder: FormBuilder,
    private messageService: MessageService,
    private router: Router,
    private geolocationService: GeolocationService
  ) {}

  ngOnInit(): void {
    this.emergencyForm = this.formBuilder.group({
      type: ['', Validators.required],
      description: ['', Validators.required],
      severity: ['', Validators.required],
      location: ['', Validators.required],
    });
  }

  onSubmit(): void {}

  getLocation(): void {
    this.geolocationService
      .getCurrentLocation()
      .then((position) => {
        const lat = position.coords.latitude;
        const lon = position.coords.longitude;
        console.log('Current Position:', lat, lon);
      })
      .catch((error) => {
        console.error('Error getting location', error);
      });
  }
}
