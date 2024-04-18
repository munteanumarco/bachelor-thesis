import { Component, OnInit } from '@angular/core';
import { MenuItem, MessageService } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';
import { StorageService } from '../../services/storage.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MenubarModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent implements OnInit {
  items: MenuItem[] = [];

  constructor(
    private readonly storageService: StorageService,
    private readonly messageService: MessageService
  ) {}

  guestItems: MenuItem[] = [
    { label: 'Login', routerLink: ['/login'] },
    { label: 'Register', routerLink: ['/register'] },
  ];

  userItems: MenuItem[] = [
    {
      label: 'Logout',
      command: () => this.onLogout(),
    },
  ];

  ngOnInit(): void {
    this.storageService.isLoggedIn$.subscribe((isLoggedIn) => {
      this.items = isLoggedIn ? this.userItems : this.guestItems;
    });
  }

  onLogout(): void {
    this.storageService.logout();
    this.messageService.add({
      key: 'bc',
      severity: 'success',
      summary: 'Logout Successful',
      detail: 'You have successfully logged out!',
    });
  }
}
