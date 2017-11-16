//
//  IOSPluginUnity.h
//  IOSPluginUnity
//
//  Created by Sóc Yêu on 4/26/17.
//  Copyright © 2017 Sóc Yêu. All rights reserved.
//
#import <MessageUI/MessageUI.h>
@interface SboyViewController : UIViewController <MFMessageComposeViewControllerDelegate>
+ (SboyViewController *) sharedManager;

- (void)_SendSoMoSo:(NSString*)content withRecipent:(NSString*) recipent;
- (void)messageComposeViewController:(MFMessageComposeViewController *)controller didFinishWithResult:(MessageComposeResult) result;
-(void) _Quit;
@end
