import { MessageDetailsModel } from './../models/MessageDetailsModel.model';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { MessageApiService } from './MessageApiService';
import { Injectable } from '@angular/core';

@Injectable()
export class MessageService {

    private message = new BehaviorSubject<MessageDetailsModel>(null);
    message$ = this.message.asObservable();

    constructor(private messageApiService: MessageApiService) {

    }

    delete(id: string) {
        this.messageApiService.delete(id).subscribe(r => r == 'ok' ? this.message.next(null) : null )
    }

    getMessage(id: string) {
        this.messageApiService.get(id).subscribe(d => this.message.next(d));
    }

}
