import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../app.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  isExpanded = false;
  constructor(private app: AppService, private router: Router) {
    //  this.app.getMsg();

  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  onSelect(serverip: string,type : string) {

    this.router.navigate(['/serveruserlist'],{queryParams:{serverip:serverip,type:type}});
  }
  onSearch(userid:string)
  {
    this.router.navigate(['/serveruserlist'],{queryParams:{serverip:userid,type:"2"}});
  }
}
