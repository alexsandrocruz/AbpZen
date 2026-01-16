import { useState } from "react";
import { Link } from "wouter";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { KeyRound, Loader2, Mail, ArrowLeft, CheckCircle2 } from "lucide-react";
import { apiClient } from "@/lib/abp/api-client";

export default function ForgotPasswordPage() {
    const [email, setEmail] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [isSent, setIsSent] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsLoading(true);
        setError(null);

        try {
            await apiClient.post("/api/account/send-password-reset-code", {
                email: email,
                appName: "Angular"
            });
            setIsSent(true);
        } catch (err: any) {
            const message = err.response?.data?.error?.message || "Failed to send reset link. Please check your email.";
            setError(message);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-background p-4 relative overflow-hidden">
            <Card className="w-full max-w-md border-none shadow-2xl bg-card/50 backdrop-blur-xl">
                <CardHeader className="text-center">
                    <div className="flex justify-center mb-4">
                        <div className="size-12 rounded-xl bg-primary text-primary-foreground flex items-center justify-center shadow-lg shadow-primary/20">
                            <KeyRound className="size-6" />
                        </div>
                    </div>
                    <CardTitle className="text-2xl font-display font-bold">
                        {isSent ? "Check your email" : "Forgot password?"}
                    </CardTitle>
                    <CardDescription>
                        {isSent
                            ? `We've sent a password reset link to ${email}`
                            : "No worries, we'll send you reset instructions."}
                    </CardDescription>
                </CardHeader>

                {isSent ? (
                    <CardContent className="space-y-4 text-center">
                        <div className="flex justify-center py-2">
                            <CheckCircle2 className="size-16 text-green-500 animate-in zoom-in duration-300" />
                        </div>
                        <p className="text-sm text-muted-foreground">
                            Did not receive the email? Check your spam folder or try again in a few minutes.
                        </p>
                    </CardContent>
                ) : (
                    <form onSubmit={handleSubmit}>
                        <CardContent className="space-y-4">
                            {error && (
                                <div className="p-3 text-sm text-destructive bg-destructive/10 border border-destructive/20 rounded-md">
                                    {error}
                                </div>
                            )}
                            <div className="space-y-2">
                                <Label htmlFor="email">Email Address</Label>
                                <div className="relative">
                                    <Mail className="absolute left-3 top-2.5 size-4 text-muted-foreground" />
                                    <Input
                                        id="email"
                                        type="email"
                                        placeholder="name@example.com"
                                        value={email}
                                        onChange={(e) => setEmail(e.target.value)}
                                        required
                                        disabled={isLoading}
                                        className="pl-10 bg-background/50"
                                    />
                                </div>
                            </div>
                        </CardContent>
                        <CardFooter className="flex flex-col gap-4">
                            <Button type="submit" className="w-full h-11 text-base shadow-lg shadow-primary/20" disabled={isLoading}>
                                {isLoading ? (
                                    <>
                                        <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                                        Sending instructions...
                                    </>
                                ) : (
                                    "Reset password"
                                )}
                            </Button>
                        </CardFooter>
                    </form>
                )}

                <div className="px-6 pb-6 text-center">
                    <Link href="/auth/login" className="inline-flex items-center text-sm text-muted-foreground hover:text-primary transition-colors">
                        <ArrowLeft className="mr-2 size-4" />
                        Back to login
                    </Link>
                </div>
            </Card>

            {/* Background decoration */}
            <div className="fixed inset-0 -z-10 bg-[radial-gradient(ellipse_at_top_right,_var(--tw-gradient-stops))] from-primary/10 via-background to-background" />
            <div className="fixed -top-24 -left-24 size-96 bg-primary/5 rounded-full blur-3xl" />
            <div className="fixed -bottom-24 -right-24 size-96 bg-primary/10 rounded-full blur-3xl" />
        </div>
    );
}
