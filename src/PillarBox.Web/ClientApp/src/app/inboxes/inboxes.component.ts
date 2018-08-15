import { Component, OnInit } from '@angular/core';
import { trigger,style,transition,animate,keyframes,query,stagger,state } from '@angular/animations';
import { InboxService } from '../services/inbox.service';
import { UserInboxModel } from '../models/UserInboxModel.model';

@Component({
  selector: 'app-inboxes',
  templateUrl: './inboxes.component.html',
  styleUrls: ['./inboxes.component.scss'],
  animations: [
    trigger('openClose', [
      state('true', style({ height: '*' })),
      state('false', style({ height: '0px' })),
      transition('false <=> true', animate(200))
    ]),

    trigger('inboxAddRemove', [
      transition('* => *', [

        query(':enter', style({ opacity: 0, height: '0' }), {optional: true}),
        
        query(':enter', stagger('0ms', [
          animate('.2s ease-in', keyframes([
            style({opacity: 0, height: '0', offset: 0}),
            style({opacity: 1, height: '*',     offset: 1.0}),
          ]))]), {optional: true})
          ,
        query(':leave', stagger('0ms', [
          animate('.2s ease-out', keyframes([
            style({opacity: 1, height: '*', offset: 0}),
            style({opacity: 0, height: '0',     offset: 1.0}),
          ]))]), {optional: true})
      ])
    ])
  ]
})
export class InboxesComponent implements OnInit {

  inboxes: Array<UserInboxModel>;
  isOpen: {};

  constructor(private inboxService: InboxService) {

    // store open status
    this.isOpen = {};

  }

  starredInboxes(input:UserInboxModel[]):UserInboxModel[] {
    var starred = new Array<UserInboxModel>();
    var path = [];

    function RecurseInboxes(list:UserInboxModel[]) {
      for(let i of list){
        path.push(i.inboxName);
        if(i.starred){
          i.fullPath = path.join('/');
          starred.push(i);
        }
        RecurseInboxes(i.children);
        path.pop();
      }
    }
    RecurseInboxes(input);

    return starred;
  }

  toggleStar(item:UserInboxModel)
  {
    item.starred = !item.starred;
    this.inboxService.setStar(item.inboxId, item.starred);
  }

  ngOnInit() {
    this.inboxService.inboxRoot.subscribe(res => this.inboxes = res);
  }

}
