//
//  IOSPluginUnity.m
//  IOSPluginUnity
//
//  Created by Sóc Yêu on 3/29/17.
//  Copyright © 2017 Sóc Yêu. All rights reserved.
//

#import "IOSPluginUnity.h"
#include <Foundation/Foundation.h>

@implementation SboyViewController
+ (SboyViewController *) sharedManager {
    static dispatch_once_t once;
    static SboyViewController *instance;
    dispatch_once(&once, ^ {
        instance = [[SboyViewController alloc] init];
    });
    
    return instance;
}

- (void)sendSoMoSo:(NSString*)content withRecipent:(NSString*)recipent{
    if(![MFMessageComposeViewController canSendText]) {
        UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Your device doesn't support!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
        [warningAlert show];
        return;
    }
    
    NSString *number = [NSString stringWithFormat:@"%@", recipent];
    NSArray *recipents = @[number];
    NSString *message = [NSString stringWithFormat:@"%@", content];
    
    NSLog(@"Number:  %@  \nContent:   %@", number, message);
    
    MFMessageComposeViewController *messageController = [[MFMessageComposeViewController alloc] init];
    messageController.messageComposeDelegate = self;
    [messageController setRecipients:recipents];
    [messageController setBody:message];
    UIViewController *uiView =  UnityGetGLViewController();
    // Present message view controller on screen
    [uiView presentViewController:messageController animated:YES completion:nil];
    
}

- (void)messageComposeViewController:(MFMessageComposeViewController *)controller didFinishWithResult:(MessageComposeResult) result
{
    switch (result) {
        case MessageComposeResultCancelled:{
            UIAlertView *cancelAlert = [[UIAlertView alloc] initWithTitle:@"Thông báo" message:@"Huỷ tin nhắn!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [cancelAlert show];
        }
            break;
        case MessageComposeResultFailed:
        {
            UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Thông báo" message:@"Gửi tin nhắn thất bại!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [warningAlert show];
            break;
        }
            
        case MessageComposeResultSent:
        {
            UIAlertView *sussceslert = [[UIAlertView alloc] initWithTitle:@"Thông báo" message:@"Gửi tin nhắn thành công!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [sussceslert show];
        }
            break;
            
        default:
            break;
    }
    
    UIViewController *uiView =  UnityGetGLViewController();
    [uiView dismissViewControllerAnimated:YES completion:nil];
}
-(void) QuitApp{
    exit(0);
}
@end
// Helper method to create C string copy
NSString* CreateNSString (const char* string) {
    if (string) {
        return [NSString stringWithUTF8String: string];
    } else {
        return [NSString stringWithUTF8String: ""];
    }
}

extern "C" {
    void _SendSoMoSo(const char *recipent, const char *content);
    void _QuitApp();
}

void _SendSoMoSo(const char *recipent, const char *content){
    [[SboyViewController sharedManager]  sendSoMoSo:CreateNSString(content) withRecipent:CreateNSString(recipent)];
}
void _QuitApp(){
    [[SboyViewController sharedManager] QuitApp];
}
