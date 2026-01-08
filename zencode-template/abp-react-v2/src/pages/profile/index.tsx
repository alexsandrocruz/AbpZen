import { useState } from "react";
import { useAuth } from "@/lib/abp/auth";
import { useAbpConfig } from "@/lib/abp/config";
import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import {
    User,
    Lock,
    ShieldCheck,
    History,
    Save,
    Camera,
    Mail,
    Smartphone,
    CheckCircle2,
    Clock,
    Laptop
} from "lucide-react";
import { apiClient } from "@/lib/abp/api-client";
import { toast } from "sonner";

export default function ProfilePage() {
    const { user } = useAuth();
    const { config } = useAbpConfig();
    const [activeTab, setActiveTab] = useState("general");
    const [isUpdating, setIsUpdating] = useState(false);

    // Form states
    const [name, setName] = useState(user?.name || "");
    const [surName, setSurName] = useState(config?.currentUser?.surName || "");
    const [email, setEmail] = useState(user?.email || "");
    const [userName, setUserName] = useState(user?.userName || "");

    const handleUpdateProfile = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsUpdating(true);
        try {
            await apiClient.put("/api/account/my-profile", {
                userName,
                email,
                name,
                surname: surName,
                concurrencyStamp: "" // Usually obtained from a GET profile first
            });
            toast.success("Profile updated successfully");
        } catch (err) {
            toast.error("Failed to update profile");
        } finally {
            setIsUpdating(false);
        }
    };

    return (
        <AppShell>
            <div className="max-w-5xl mx-auto space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-500">
                <div>
                    <h1 className="text-3xl font-display font-bold text-foreground">Account Settings</h1>
                    <p className="text-muted-foreground mt-1">Manage your profile information, security, and activity.</p>
                </div>

                <Tabs value={activeTab} onValueChange={setActiveTab} className="space-y-6">
                    <TabsList className="bg-muted/50 p-1">
                        <TabsTrigger value="general" className="gap-2">
                            <User size={16} />
                            General
                        </TabsTrigger>
                        <TabsTrigger value="security" className="gap-2">
                            <ShieldCheck size={16} />
                            Security
                        </TabsTrigger>
                        <TabsTrigger value="sessions" className="gap-2">
                            <History size={16} />
                            Activity
                        </TabsTrigger>
                    </TabsList>

                    <TabsContent value="general" className="space-y-6">
                        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                            {/* Profile card */}
                            <Card className="col-span-1 border-none bg-card/60 backdrop-blur shadow-xl">
                                <CardHeader className="text-center">
                                    <div className="relative group mx-auto mb-4">
                                        <Avatar className="size-24 border-4 border-background shadow-xl">
                                            <AvatarImage src={`https://api.dicebear.com/7.x/avataaars/svg?seed=${user?.userName}`} />
                                            <AvatarFallback className="text-2xl">{user?.name?.[0]}{user?.userName?.[0]}</AvatarFallback>
                                        </Avatar>
                                        <button className="absolute bottom-0 right-0 p-1.5 bg-primary text-primary-foreground rounded-full shadow-lg opacity-0 group-hover:opacity-100 transition-opacity">
                                            <Camera size={14} />
                                        </button>
                                    </div>
                                    <CardTitle className="font-display font-bold">{user?.name || user?.userName}</CardTitle>
                                    <CardDescription className="flex items-center justify-center gap-1">
                                        <CheckCircle2 size={12} className="text-primary" />
                                        Verified Account
                                    </CardDescription>
                                </CardHeader>
                                <CardContent className="space-y-2">
                                    <div className="flex justify-between text-sm">
                                        <span className="text-muted-foreground">Status</span>
                                        <span className="font-medium text-green-500">Active</span>
                                    </div>
                                    <div className="flex justify-between text-sm">
                                        <span className="text-muted-foreground">Member since</span>
                                        <span className="font-medium">Jan 2024</span>
                                    </div>
                                </CardContent>
                                <CardFooter>
                                    <Button variant="outline" className="w-full h-9 text-xs" onClick={() => toast.info("Avatar upload is coming soon!")}>
                                        Change Avatar
                                    </Button>
                                </CardFooter>
                            </Card>

                            {/* Form card */}
                            <Card className="col-span-1 md:col-span-2 border-none bg-card/60 backdrop-blur shadow-xl">
                                <CardHeader>
                                    <CardTitle className="font-display font-bold">Personal Information</CardTitle>
                                    <CardDescription>Update your profile details and contact info.</CardDescription>
                                </CardHeader>
                                <form onSubmit={handleUpdateProfile}>
                                    <CardContent className="space-y-4">
                                        <div className="grid grid-cols-2 gap-4">
                                            <div className="space-y-2">
                                                <Label htmlFor="name">First Name</Label>
                                                <Input
                                                    id="name"
                                                    value={name}
                                                    onChange={e => setName(e.target.value)}
                                                    className="bg-background/40"
                                                />
                                            </div>
                                            <div className="space-y-2">
                                                <Label htmlFor="surname">Last Name</Label>
                                                <Input
                                                    id="surname"
                                                    value={surName}
                                                    onChange={e => setSurName(e.target.value)}
                                                    className="bg-background/40"
                                                />
                                            </div>
                                        </div>
                                        <div className="space-y-2">
                                            <Label htmlFor="username">Username</Label>
                                            <Input
                                                id="username"
                                                value={userName}
                                                onChange={e => setUserName(e.target.value)}
                                                className="bg-background/40"
                                            />
                                        </div>
                                        <div className="space-y-2">
                                            <Label htmlFor="email">Email address</Label>
                                            <div className="relative">
                                                <Mail className="absolute left-3 top-2.5 size-4 text-muted-foreground" />
                                                <Input
                                                    id="email"
                                                    type="email"
                                                    value={email}
                                                    onChange={e => setEmail(e.target.value)}
                                                    className="pl-10 bg-background/40"
                                                />
                                            </div>
                                        </div>
                                    </CardContent>
                                    <CardFooter className="justify-end border-t border-border/50 pt-6">
                                        <Button type="submit" disabled={isUpdating} className="gap-2 px-8 shadow-lg shadow-primary/20">
                                            {isUpdating ? <Clock className="animate-spin size-4" /> : <Save size={16} />}
                                            Save Changes
                                        </Button>
                                    </CardFooter>
                                </form>
                            </Card>
                        </div>
                    </TabsContent>

                    <TabsContent value="security" className="space-y-6">
                        <Card className="border-none bg-card/60 backdrop-blur shadow-xl max-w-2xl">
                            <CardHeader>
                                <CardTitle className="font-display font-bold">Update Password</CardTitle>
                                <CardDescription>Ensure your account is using a long, random password to stay secure.</CardDescription>
                            </CardHeader>
                            <form>
                                <CardContent className="space-y-4">
                                    <div className="space-y-2">
                                        <Label htmlFor="current-password">Current Password</Label>
                                        <Input id="current-password" type="password" className="bg-background/40" />
                                    </div>
                                    <div className="space-y-2">
                                        <Label htmlFor="new-password">New Password</Label>
                                        <Input id="new-password" type="password" className="bg-background/40" />
                                    </div>
                                    <div className="space-y-2">
                                        <Label htmlFor="confirm-password">Confirm New Password</Label>
                                        <Input id="confirm-password" type="password" className="bg-background/40" />
                                    </div>
                                </CardContent>
                                <CardFooter className="justify-end border-t border-border/50 pt-6">
                                    <Button type="button" className="gap-2 shadow-lg shadow-primary/20" onClick={() => toast.promise(new Promise(r => setTimeout(r, 1000)), { loading: 'Updating...', success: 'Password changed!', error: 'Failed' })}>
                                        <Lock size={16} />
                                        Update Password
                                    </Button>
                                </CardFooter>
                            </form>
                        </Card>

                        <Card className="border-none bg-destructive/5 dark:bg-destructive/10 border-destructive/20 max-w-2xl">
                            <CardHeader>
                                <CardTitle className="text-destructive font-display font-bold">Two-Factor Authentication</CardTitle>
                                <CardDescription>Add an extra layer of security to your account.</CardDescription>
                            </CardHeader>
                            <CardContent>
                                <div className="flex items-center gap-4 py-2">
                                    <Smartphone className="size-10 text-muted-foreground" />
                                    <div>
                                        <p className="font-medium">Authenticator App</p>
                                        <p className="text-sm text-muted-foreground text-balance">Use an app like Google Authenticator or Microsoft Authenticator to generate verification codes.</p>
                                    </div>
                                    <Button variant="outline" className="ml-auto">Enable</Button>
                                </div>
                            </CardContent>
                        </Card>
                    </TabsContent>

                    <TabsContent value="sessions" className="space-y-6">
                        <Card className="border-none bg-card/60 backdrop-blur shadow-xl">
                            <CardHeader>
                                <CardTitle className="font-display font-bold text-xl">Active Sessions</CardTitle>
                                <CardDescription>Devices that are currently logged into your account.</CardDescription>
                            </CardHeader>
                            <CardContent className="space-y-6">
                                <div className="flex items-center gap-4 p-4 rounded-xl bg-background/40 border border-border/30">
                                    <div className="size-10 rounded-lg bg-primary/10 text-primary flex items-center justify-center">
                                        <Laptop size={20} />
                                    </div>
                                    <div className="flex-1">
                                        <p className="font-bold flex items-center gap-2">
                                            Macintosh <span className="text-[10px] bg-green-500/20 text-green-500 px-2 py-0.5 rounded-full uppercase tracking-wider">Current Device</span>
                                        </p>
                                        <p className="text-xs text-muted-foreground">Chrome · San Diego, CA (192.168.1.1)</p>
                                    </div>
                                    <Button variant="ghost" size="sm" className="text-muted-foreground">Log out</Button>
                                </div>

                                <div className="flex items-center gap-4 p-4 rounded-xl bg-muted/20 border border-border/10 opacity-60">
                                    <div className="size-10 rounded-lg bg-muted-foreground/10 text-muted-foreground flex items-center justify-center">
                                        <Smartphone size={20} />
                                    </div>
                                    <div className="flex-1">
                                        <p className="font-bold">iPhone 15 Pro</p>
                                        <p className="text-xs text-muted-foreground">App · San Diego, CA (192.168.1.5)</p>
                                    </div>
                                    <Button variant="ghost" size="sm" className="text-muted-foreground">Log out</Button>
                                </div>
                            </CardContent>
                            <CardFooter className="justify-between bg-muted/20 rounded-b-xl border-t border-border/10">
                                <span className="text-xs text-muted-foreground italic">You were last active 2 minutes ago.</span>
                                <Button variant="outline" size="sm" className="text-destructive hover:bg-destructive/10">Log out all other devices</Button>
                            </CardFooter>
                        </Card>
                    </TabsContent>
                </Tabs>
            </div>
        </AppShell>
    );
}
