import { Component, Input, ViewEncapsulation } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/models/member';
import { MembersService } from 'src/app/services/members.service';
import { PresenceService } from 'src/app/services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent {
  @Input() member: Member | undefined;

  constructor(private memberService: MembersService, private toastr: ToastrService,
    public presenceService: PresenceService) {
  }

  sendRequest(member: Member) {
    this.memberService.sendRequest(member.userName).subscribe({
      next: () => this.toastr.success('Friend request sent to ' + member.knownAs)
    });
  }

}
