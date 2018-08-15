import { MessageSummaryModel } from './../models/MessageSummaryModel.model';
import { ViewInboxModel } from './../models/ViewInboxModel.model';
import { InboxService } from './../services/inbox.service';
import { Component, OnInit } from '@angular/core';
import { trigger, style, transition, animate, keyframes, query, stagger, state } from '@angular/animations';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmService } from '../utils/confirm-modal-and-service';

@Component({
    selector: 'app-inbox',
    templateUrl: './inbox.component.html',
    styleUrls: ['./inbox.component.scss'],
    animations: [

        trigger('messageAddRemove', [
            transition('* => *', [

                query(':enter', style({ opacity: 0, height: '0' }), { optional: true }),

                query(':enter', stagger('0ms', [
                    animate('.2s ease-in', keyframes([
                        style({ opacity: 0, height: '0', offset: 0 }),
                        style({ opacity: 1, height: '*', offset: 1.0 }),
                    ]))]), { optional: true })
                ,
                query(':leave', stagger('0ms', [
                    animate('.2s ease-out', keyframes([
                        style({ opacity: 1, height: '*', offset: 0 }),
                        style({ opacity: 0, height: '0', offset: 1.0 }),
                    ]))]), { optional: true })
            ])
        ])
    ]
})
export class InboxComponent implements OnInit {

    inboxDetails: ViewInboxModel;

    id: string;

    constructor(private route: ActivatedRoute,
        private router: Router,
        private inboxService: InboxService,
        private confirmService: ConfirmService) {

        this.route.params.subscribe(params => {

            this.inboxDetails = null;

            this.id = params['id'];
            this.loadInbox();
        });

        this.inboxService.inboxContent
            .subscribe(i => {
                if (i == null && this.inboxDetails != null) {
                    // record deleted
                    this.router.navigate(['']);
                }
                this.inboxDetails = i;
            }
        );

    }

    loadInbox() {
        this.inboxService.switchInbox(this.id);
    }

    panelScrolled(event) {
        if (event.srcElement.scrollTop + event.srcElement.clientHeight >= event.srcElement.scrollHeight &&
            this.inboxDetails.messages.hasNextPage
        ) {
            this.inboxService.nextPage();
        }
    }

    delete() {
        this.confirmService.confirm({ title: 'Confirm deletion', message: 'Do you really want to delete this inbox and all emails in it? This can not be undone.' }).then(
            () => {
                this.inboxService.delete(this.inboxDetails.id);
            },
            () => {
            });
    }

    ngOnInit() {
    }

    getName(input: string) {

        return input == null || input.indexOf("<") < 0 ? input : input.match(/([^<]*)/)[1].replace(/"/g, '');
    }

}
