import { Component } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { User } from 'src/app/models/user';
import { Observable, of } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MembersService } from 'src/app/services/members.service';
import { UserParams } from 'src/app/models/userParams';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']

})
export class NavComponent {
  model: any = {};
  currentUser$: Observable<User | null> = of(null);
  public isCollapsed: boolean = true;
  userParams: UserParams | undefined;

  constructor(public accountService: AccountService, private memberService: MembersService, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: () => {
        this.memberService.resetUserParams();
        this.memberService.memberCache.clear();
        this.router.navigateByUrl('/members');
        this.model = {}
      }
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/')
  }
}
