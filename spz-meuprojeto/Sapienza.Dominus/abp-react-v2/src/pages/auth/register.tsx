import { useState } from "react";
import { useLocation, Link } from "wouter";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Loader2, UserPlus, CheckCircle2, Building2, Globe, FileText } from "lucide-react";
import { apiClient } from "@/lib/abp/api-client";
import { resolveTenantFromHostname, isRootDomain } from "@/lib/abp/tenant";
import { Checkbox } from "@/components/ui/checkbox";

export default function RegisterPage() {
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [workspaceName, setWorkspaceName] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [isSuccess, setIsSuccess] = useState(false);
    const [acceptedTerms, setAcceptedTerms] = useState(false);
    const [, setLocation] = useLocation();

    const tenant = resolveTenantFromHostname();
    const isRoot = isRootDomain();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsLoading(true);
        setError(null);

        try {
            if (isRoot) {
                // Flow 1: Create Workspace (Tenant) + Admin User
                await apiClient.post("/api/multi-tenancy/tenants/register", {
                    name: workspaceName,
                    adminEmailAddress: email,
                    adminPassword: password
                });
            } else {
                // Flow 2: Register User to existing Tenant
                await apiClient.post("/api/account/register", {
                    userName: username,
                    emailAddress: email,
                    password: password,
                    appName: "React"
                });
            }
            setIsSuccess(true);
        } catch (err: any) {
            const message = err.response?.data?.error?.message || "Registration failed. Please check your details.";
            setError(message);
        } finally {
            setIsLoading(false);
        }
    };

    if (isSuccess) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-background p-4">
                <Card className="w-full max-w-md text-center">
                    <CardHeader>
                        <div className="flex justify-center mb-4">
                            <div className="size-16 rounded-full bg-green-500/10 text-green-500 flex items-center justify-center shadow-inner">
                                <CheckCircle2 className="size-10 animate-in zoom-in duration-300" />
                            </div>
                        </div>
                        <CardTitle className="text-2xl font-display font-bold">
                            {isRoot ? "Workspace Created!" : "Awaiting Approval"}
                        </CardTitle>
                        <CardDescription>
                            {isRoot
                                ? "Your new workspace is being prepared. You can now log in."
                                : "Your registration was successful and is now being reviewed."}
                        </CardDescription>
                    </CardHeader>
                    <CardContent className="space-y-4">
                        <p className="text-sm text-muted-foreground">
                            {isRoot
                                ? `You will be redirected to your new dashboard shortly.`
                                : "An administrator will evaluate your access request soon. You will receive an email once approved."}
                        </p>
                    </CardContent>
                    <CardFooter className="flex flex-col gap-3">
                        {isRoot ? (
                            <Button
                                onClick={() => {
                                    const format = import.meta.env.VITE_TENANT_DOMAIN_FORMAT || "{0}.zensuite.com.br";
                                    const subdomain = workspaceName.toLowerCase().replace(/\s+/g, '-');
                                    const newDomain = format.replace("{0}", subdomain);
                                    window.location.href = `${window.location.protocol}//${newDomain}/auth/login`;
                                }}
                                className="w-full h-11 bg-green-600 hover:bg-green-700 shadow-lg shadow-green-500/20"
                            >
                                Go to Workspace
                            </Button>
                        ) : (
                            <Button onClick={() => setLocation("/auth/login")} className="w-full h-11">
                                Back to Login
                            </Button>
                        )}
                        <Button variant="ghost" onClick={() => setLocation("/auth/login")} className="w-full">
                            Cancel
                        </Button>
                    </CardFooter>
                </Card>
            </div>
        );
    }

    return (
        <div className="min-h-screen flex items-center justify-center bg-background p-4">
            <Card className="w-full max-w-md border-none shadow-2xl bg-card/50 backdrop-blur-xl">
                <CardHeader className="text-center">
                    <div className="flex justify-center mb-4">
                        <div className="size-12 rounded-xl bg-primary text-primary-foreground flex items-center justify-center shadow-lg shadow-primary/20">
                            <UserPlus className="size-6" />
                        </div>
                    </div>
                    <CardTitle className="text-2xl font-display font-bold">
                        {isRoot ? "Start your Workspace" : "Join the Platform"}
                    </CardTitle>
                    <CardDescription>
                        {isRoot
                            ? "Register your organization to start using the platform"
                            : `Creating an account for ${tenant || 'current workspace'}`}
                    </CardDescription>
                </CardHeader>
                <form onSubmit={handleSubmit}>
                    <CardContent className="space-y-4">
                        {error && (
                            <div className="p-3 text-sm text-destructive bg-destructive/10 border border-destructive/20 rounded-md">
                                {error}
                            </div>
                        )}
                        {isRoot && (
                            <div className="space-y-2">
                                <Label htmlFor="workspace">Workspace Name (Tenant)</Label>
                                <div className="relative">
                                    <Building2 className="absolute left-3 top-2.5 size-4 text-muted-foreground" />
                                    <Input
                                        id="workspace"
                                        placeholder="Company Name"
                                        value={workspaceName}
                                        onChange={(e) => setWorkspaceName(e.target.value)}
                                        required
                                        className="pl-10 bg-background/50"
                                    />
                                </div>
                                <p className="text-[10px] text-muted-foreground flex items-center gap-1">
                                    <Globe className="size-3" />
                                    Your workspace will be accessible at {workspaceName.toLowerCase().replace(/\s+/g, '-')}.zensuite.com.br
                                </p>
                            </div>
                        )}
                        <div className="space-y-2">
                            <Label htmlFor="username">Username</Label>
                            <Input
                                id="username"
                                type="text"
                                placeholder="johndoe"
                                value={username}
                                onChange={(e) => setUsername(e.target.value)}
                                required
                                disabled={isLoading}
                                className="bg-background/50"
                            />
                        </div>
                        <div className="space-y-2">
                            <Label htmlFor="email">Email Address</Label>
                            <Input
                                id="email"
                                type="email"
                                placeholder="john@example.com"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                required
                                disabled={isLoading}
                                className="bg-background/50"
                            />
                        </div>
                        <div className="space-y-2">
                            <Label htmlFor="password">Password</Label>
                            <Input
                                id="password"
                                type="password"
                                placeholder="••••••••"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required
                                disabled={isLoading}
                                className="bg-background/50"
                            />
                        </div>

                        {/* GDPR/LGPD Consent */}
                        <div className="flex items-start space-x-3 pt-2">
                            <Checkbox
                                id="terms"
                                checked={acceptedTerms}
                                onCheckedChange={(checked) => setAcceptedTerms(checked === true)}
                                disabled={isLoading}
                                className="mt-0.5"
                            />
                            <div className="grid gap-1.5 leading-none">
                                <label
                                    htmlFor="terms"
                                    className="text-sm font-medium leading-tight cursor-pointer"
                                >
                                    Aceito os Termos de Uso e Política de Privacidade
                                </label>
                                <p className="text-xs text-muted-foreground flex items-center gap-1">
                                    <FileText className="size-3" />
                                    <Link href="/legal/terms" className="text-primary hover:underline">Termos de Uso</Link>
                                    {" • "}
                                    <Link href="/legal/privacy" className="text-primary hover:underline">Política de Privacidade</Link>
                                </p>
                            </div>
                        </div>
                    </CardContent>
                    <CardFooter className="flex flex-col gap-4">
                        <Button type="submit" className="w-full h-11 text-base shadow-lg shadow-primary/20" disabled={isLoading || !acceptedTerms}>
                            {isLoading ? (
                                <>
                                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                                    Creating account...
                                </>
                            ) : (
                                "Register"
                            )}
                        </Button>
                        <p className="text-sm text-muted-foreground text-center">
                            Already have an account?{" "}
                            <Link href="/auth/login" className="text-primary font-medium hover:underline">
                                Sign in
                            </Link>
                        </p>
                    </CardFooter>
                </form>
            </Card>

            {/* Background decoration */}
            <div className="fixed inset-0 -z-10 bg-[radial-gradient(ellipse_at_top_right,_var(--tw-gradient-stops))] from-primary/10 via-background to-background" />
        </div>
    );
}
