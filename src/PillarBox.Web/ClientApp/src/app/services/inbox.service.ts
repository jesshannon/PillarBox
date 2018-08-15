import { NotificationsService } from './notifications.service';
import { ViewInboxModel } from './../models/ViewInboxModel.model';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { InboxApiService } from './InboxApiService';
import { UserInboxModel } from '../models/UserInboxModel.model';
import 'rxjs/add/operator/switch';

@Injectable()
export class InboxService {

    private inboxes = new BehaviorSubject<Array<UserInboxModel>>([]);
    inboxRoot = this.inboxes.asObservable();

    private inbox = new BehaviorSubject<ViewInboxModel>(null);
    inboxContent = this.inbox.asObservable();

    private currentInbox: ViewInboxModel;
    private currentPage: number = 0;
    private inboxId: string;

    constructor(private inboxApi: InboxApiService, private notificationService: NotificationsService) {

        this.loadInboxes();

        notificationService.inboxRoot.subscribe(i => this.inboxes.next(i));

        notificationService.messages$.subscribe(m => {
            if (m != null && m.inboxId == this.inboxId) {
                m.isNew = true;
                this.currentInbox.messages.items.unshift(m);
                this.inbox.next(this.currentInbox);
            }
        });
    }

    private loadInboxes() {
        this.inboxApi.getInboxTree().subscribe(response => {
            this.inboxes.next(response);
        });
    }

    switchInbox(id: string) {
        this.inboxId = id;
        this.refreshInbox();
    }

    refreshInbox() {
        this.currentPage = 0;

        this.notificationService.subscribeInbox(this.inboxId);

        this.inboxApi.get(this.inboxId, this.currentPage)
            .subscribe(r => {
                this.currentInbox = r;
                this.inbox.next(r);
            });
    }

    nextPage() {
        if (this.inbox.value.messages.pageIndex == this.currentPage) {
            this.inboxApi.get(this.inboxId, this.currentPage + 1)
                .subscribe(r => {
                    if (this.currentInbox == null) {
                        this.currentInbox = r;
                    }
                    else {
                        this.currentInbox.messages.pageIndex = r.messages.pageIndex;
                        this.currentPage = r.messages.pageIndex;
                        this.currentInbox.messages.hasNextPage = r.messages.hasNextPage;

                        this.currentInbox.messages.items.forEach(c => {
                            var item = r.messages.items.findIndex(m => m.id == c.id);
                            if (item > -1) {
                                r.messages.items.splice(item, 1);
                            }
                        });

                        r.messages.items
                            .forEach(m => this.currentInbox.messages.items.push(m));
                        this.inbox.next(this.currentInbox);
                    }
                });
        }
    }

    delete(id: string) {
        this.inboxApi.delete(id).subscribe(r => {
            if (r == "ok") {
                this.inbox.next(null);
                this.loadInboxes();
            }
        });
    }

    setStar(id: string, isStarred: boolean) {
        this.inboxApi.setStarred(id, isStarred).subscribe(r => { });
    }

}
