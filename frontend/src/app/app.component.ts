import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { RouteService } from './services/route.service';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { FormsModule } from '@angular/forms';
import { SpinnerComponent } from './shared/spinner/spinner.component';
import { WebSocketService } from './services/web-socket.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    NavbarComponent,
    ToastModule,
    FormsModule,
    RouterModule,
    SpinnerComponent,
  ],
  providers: [MessageService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'Sky Sentinel';
  showNavbar = true;
  private messagesSubscription!: Subscription;
  
  constructor(
    private routeService: RouteService,
    private webSocketService: WebSocketService,
    private messageService: MessageService
  ) {
    this.routeService.currentUrl$.subscribe((url) => {
      this.setNavbarVisibility(url);
    });
  }

  ngOnInit() {
    this.messagesSubscription = this.webSocketService.messages.subscribe(
      (msg: string) => {
        const data = JSON.parse(msg);
        this.showSuccess(data);
      },
      (err) => console.error('Encountered error:', err),
      () => console.warn('Completed!')
    );
  }

  setNavbarVisibility(url: string) {
    const NO_NAVBAR_PREFIXES = [
      '/login',
      '/register',
      '/confirm-email',
      '/forgot-password',
      '/check-email',
      '/reset-password',
      '/report-emergency',
      '/chat',
      '/assistant',
      '/analysis',
    ];
    this.showNavbar = !NO_NAVBAR_PREFIXES.some((prefix) =>
      url.startsWith(prefix)
    );
  }

  ngOnDestroy() {
    this.messagesSubscription.unsubscribe();
  }

  sendMessage(message: string): void {
    this.webSocketService.send(message);
  }

  showSuccess(data: any): void {
    this.messageService.add({
      key: 'analysis',
      severity: 'success',
      summary: 'Success',
      detail: data.message,
      id: data.emergency_event_id,
    });
  }
}
