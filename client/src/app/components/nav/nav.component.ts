import { Component } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { User } from 'src/app/models/user';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']

})
export class NavComponent {
  model: any = {};
  currentUser$: Observable<User | null> = of(null);
  public isCollapsed: boolean = true;

  constructor(public accountService: AccountService) { }

  ngOnInit() {
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(response);
      },
      error: error => console.log(error)
    })
  }

  logout() {
    this.accountService.logout();
  }
}
