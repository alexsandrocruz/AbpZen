import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Switch } from "@/components/ui/switch";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Separator } from "@/components/ui/separator";
import { Settings, Globe, Mail, Shield, Save, Bell } from "lucide-react";

export default function HostSettingsPage() {
    return (
        <AppShell>
            <div className="space-y-6">
                {/* Page Header */}
                <div className="flex items-center gap-3">
                    <Settings className="h-8 w-8 text-primary" />
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">Settings</h1>
                        <p className="text-muted-foreground">
                            Configure system-wide settings
                        </p>
                    </div>
                </div>

                {/* Settings Tabs */}
                <Tabs defaultValue="general" className="space-y-6">
                    <TabsList>
                        <TabsTrigger value="general" className="gap-2">
                            <Globe className="h-4 w-4" />
                            General
                        </TabsTrigger>
                        <TabsTrigger value="email" className="gap-2">
                            <Mail className="h-4 w-4" />
                            Email
                        </TabsTrigger>
                        <TabsTrigger value="security" className="gap-2">
                            <Shield className="h-4 w-4" />
                            Security
                        </TabsTrigger>
                        <TabsTrigger value="notifications" className="gap-2">
                            <Bell className="h-4 w-4" />
                            Notifications
                        </TabsTrigger>
                    </TabsList>

                    {/* General Settings */}
                    <TabsContent value="general">
                        <Card>
                            <CardHeader>
                                <CardTitle>General Settings</CardTitle>
                                <CardDescription>
                                    Configure general application settings
                                </CardDescription>
                            </CardHeader>
                            <CardContent className="space-y-6">
                                <div className="space-y-2">
                                    <Label htmlFor="appName">Application Name</Label>
                                    <Input id="appName" defaultValue="AbpReact" />
                                    <p className="text-sm text-muted-foreground">
                                        The name displayed in the application header
                                    </p>
                                </div>

                                <Separator />

                                <div className="space-y-2">
                                    <Label htmlFor="defaultLanguage">Default Language</Label>
                                    <Input id="defaultLanguage" defaultValue="en" />
                                    <p className="text-sm text-muted-foreground">
                                        Default language for new users
                                    </p>
                                </div>

                                <Separator />

                                <div className="flex items-center justify-between">
                                    <div className="space-y-0.5">
                                        <Label>Enable Multi-tenancy</Label>
                                        <p className="text-sm text-muted-foreground">
                                            Allow multiple workspaces in the system
                                        </p>
                                    </div>
                                    <Switch defaultChecked />
                                </div>

                                <Separator />

                                <div className="flex items-center justify-between">
                                    <div className="space-y-0.5">
                                        <Label>Enable Self Registration</Label>
                                        <p className="text-sm text-muted-foreground">
                                            Allow users to register themselves
                                        </p>
                                    </div>
                                    <Switch />
                                </div>

                                <div className="flex justify-end">
                                    <Button className="gap-2">
                                        <Save className="h-4 w-4" />
                                        Save Changes
                                    </Button>
                                </div>
                            </CardContent>
                        </Card>
                    </TabsContent>

                    {/* Email Settings */}
                    <TabsContent value="email">
                        <Card>
                            <CardHeader>
                                <CardTitle>Email Settings</CardTitle>
                                <CardDescription>
                                    Configure SMTP and email notification settings
                                </CardDescription>
                            </CardHeader>
                            <CardContent className="space-y-6">
                                <div className="grid gap-4 md:grid-cols-2">
                                    <div className="space-y-2">
                                        <Label htmlFor="smtpHost">SMTP Host</Label>
                                        <Input id="smtpHost" placeholder="smtp.example.com" />
                                    </div>
                                    <div className="space-y-2">
                                        <Label htmlFor="smtpPort">SMTP Port</Label>
                                        <Input id="smtpPort" type="number" defaultValue="587" />
                                    </div>
                                </div>

                                <div className="grid gap-4 md:grid-cols-2">
                                    <div className="space-y-2">
                                        <Label htmlFor="smtpUser">Username</Label>
                                        <Input id="smtpUser" placeholder="user@example.com" />
                                    </div>
                                    <div className="space-y-2">
                                        <Label htmlFor="smtpPassword">Password</Label>
                                        <Input id="smtpPassword" type="password" />
                                    </div>
                                </div>

                                <Separator />

                                <div className="space-y-2">
                                    <Label htmlFor="senderEmail">Sender Email</Label>
                                    <Input id="senderEmail" placeholder="noreply@example.com" />
                                </div>

                                <div className="space-y-2">
                                    <Label htmlFor="senderName">Sender Name</Label>
                                    <Input id="senderName" placeholder="My Application" />
                                </div>

                                <Separator />

                                <div className="flex items-center justify-between">
                                    <div className="space-y-0.5">
                                        <Label>Enable SSL</Label>
                                        <p className="text-sm text-muted-foreground">
                                            Use SSL/TLS for SMTP connection
                                        </p>
                                    </div>
                                    <Switch defaultChecked />
                                </div>

                                <div className="flex justify-end gap-2">
                                    <Button variant="outline">Test Connection</Button>
                                    <Button className="gap-2">
                                        <Save className="h-4 w-4" />
                                        Save Changes
                                    </Button>
                                </div>
                            </CardContent>
                        </Card>
                    </TabsContent>

                    {/* Security Settings */}
                    <TabsContent value="security">
                        <Card>
                            <CardHeader>
                                <CardTitle>Security Settings</CardTitle>
                                <CardDescription>
                                    Configure password and authentication policies
                                </CardDescription>
                            </CardHeader>
                            <CardContent className="space-y-6">
                                <div className="space-y-4">
                                    <h4 className="font-medium">Password Policy</h4>

                                    <div className="grid gap-4 md:grid-cols-2">
                                        <div className="space-y-2">
                                            <Label htmlFor="minLength">Minimum Length</Label>
                                            <Input id="minLength" type="number" defaultValue="8" />
                                        </div>
                                        <div className="space-y-2">
                                            <Label htmlFor="maxLength">Maximum Length</Label>
                                            <Input id="maxLength" type="number" defaultValue="128" />
                                        </div>
                                    </div>

                                    <div className="flex items-center justify-between">
                                        <div className="space-y-0.5">
                                            <Label>Require Uppercase</Label>
                                            <p className="text-sm text-muted-foreground">
                                                Password must contain uppercase letters
                                            </p>
                                        </div>
                                        <Switch defaultChecked />
                                    </div>

                                    <div className="flex items-center justify-between">
                                        <div className="space-y-0.5">
                                            <Label>Require Lowercase</Label>
                                            <p className="text-sm text-muted-foreground">
                                                Password must contain lowercase letters
                                            </p>
                                        </div>
                                        <Switch defaultChecked />
                                    </div>

                                    <div className="flex items-center justify-between">
                                        <div className="space-y-0.5">
                                            <Label>Require Numbers</Label>
                                            <p className="text-sm text-muted-foreground">
                                                Password must contain numbers
                                            </p>
                                        </div>
                                        <Switch defaultChecked />
                                    </div>

                                    <div className="flex items-center justify-between">
                                        <div className="space-y-0.5">
                                            <Label>Require Special Characters</Label>
                                            <p className="text-sm text-muted-foreground">
                                                Password must contain special characters
                                            </p>
                                        </div>
                                        <Switch />
                                    </div>
                                </div>

                                <Separator />

                                <div className="space-y-4">
                                    <h4 className="font-medium">Lockout Policy</h4>

                                    <div className="flex items-center justify-between">
                                        <div className="space-y-0.5">
                                            <Label>Enable Lockout</Label>
                                            <p className="text-sm text-muted-foreground">
                                                Lock account after failed attempts
                                            </p>
                                        </div>
                                        <Switch defaultChecked />
                                    </div>

                                    <div className="grid gap-4 md:grid-cols-2">
                                        <div className="space-y-2">
                                            <Label htmlFor="maxAttempts">Max Failed Attempts</Label>
                                            <Input id="maxAttempts" type="number" defaultValue="5" />
                                        </div>
                                        <div className="space-y-2">
                                            <Label htmlFor="lockoutDuration">Lockout Duration (minutes)</Label>
                                            <Input id="lockoutDuration" type="number" defaultValue="15" />
                                        </div>
                                    </div>
                                </div>

                                <div className="flex justify-end">
                                    <Button className="gap-2">
                                        <Save className="h-4 w-4" />
                                        Save Changes
                                    </Button>
                                </div>
                            </CardContent>
                        </Card>
                    </TabsContent>

                    {/* Notification Settings */}
                    <TabsContent value="notifications">
                        <Card>
                            <CardHeader>
                                <CardTitle>Notification Settings</CardTitle>
                                <CardDescription>
                                    Configure system notification preferences
                                </CardDescription>
                            </CardHeader>
                            <CardContent className="space-y-6">
                                <div className="flex items-center justify-between">
                                    <div className="space-y-0.5">
                                        <Label>Email Notifications</Label>
                                        <p className="text-sm text-muted-foreground">
                                            Send email notifications for important events
                                        </p>
                                    </div>
                                    <Switch defaultChecked />
                                </div>

                                <div className="flex items-center justify-between">
                                    <div className="space-y-0.5">
                                        <Label>New User Registration</Label>
                                        <p className="text-sm text-muted-foreground">
                                            Notify admins when new users register
                                        </p>
                                    </div>
                                    <Switch defaultChecked />
                                </div>

                                <div className="flex items-center justify-between">
                                    <div className="space-y-0.5">
                                        <Label>New Workspace Created</Label>
                                        <p className="text-sm text-muted-foreground">
                                            Notify admins when new workspaces are created
                                        </p>
                                    </div>
                                    <Switch defaultChecked />
                                </div>

                                <div className="flex items-center justify-between">
                                    <div className="space-y-0.5">
                                        <Label>Security Alerts</Label>
                                        <p className="text-sm text-muted-foreground">
                                            Notify about suspicious login attempts
                                        </p>
                                    </div>
                                    <Switch defaultChecked />
                                </div>

                                <div className="flex justify-end">
                                    <Button className="gap-2">
                                        <Save className="h-4 w-4" />
                                        Save Changes
                                    </Button>
                                </div>
                            </CardContent>
                        </Card>
                    </TabsContent>
                </Tabs>
            </div>
        </AppShell>
    );
}
