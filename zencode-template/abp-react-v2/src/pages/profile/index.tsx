import { useState, useRef } from "react";
import { useAuth } from "@/lib/abp/auth";
import { useAbpConfig } from "@/lib/abp/config";
import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Badge } from "@/components/ui/badge";
import {
    User,
    Lock,
    ShieldCheck,
    Save,
    Camera,
    Mail,
    Smartphone,
    CheckCircle2,
    Clock,
    Building2,
    Fingerprint,
    Info,
    Shield,
    Trash2,
} from "lucide-react";
import { apiClient } from "@/lib/abp/api-client";
import { toast } from "sonner";
import { useProfilePicture } from "@/lib/abp/hooks/use-profile-picture";

export default function ProfilePage() {
    const { user } = useAuth();
    const { config } = useAbpConfig();
    const [activeTab, setActiveTab] = useState("info");
    const [isUpdating, setIsUpdating] = useState(false);
    const fileInputRef = useRef<HTMLInputElement>(null);
    const { pictureUrl, isUploading, uploadPicture, deletePicture } = useProfilePicture();

    // Form states
    const [name, setName] = useState(user?.name || "");
    const [surName, setSurName] = useState(config?.currentUser?.surName || "");
    const [email, setEmail] = useState(user?.email || "");
    const [userName, setUserName] = useState(user?.userName || "");
    const [phoneNumber, setPhoneNumber] = useState("");

    // Password states
    const [currentPassword, setCurrentPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    const handleUpdateProfile = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsUpdating(true);
        try {
            await apiClient.put("/api/account/my-profile", {
                userName,
                email,
                name,
                surname: surName,
                phoneNumber,
                concurrencyStamp: ""
            });
            toast.success("Perfil atualizado com sucesso!");
        } catch (err) {
            toast.error("Erro ao atualizar perfil");
        } finally {
            setIsUpdating(false);
        }
    };

    return (
        <AppShell>
            <div className="max-w-5xl mx-auto space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 pb-12">
                <div className="flex items-center justify-between">
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">Meu Perfil</h1>
                        <p className="text-muted-foreground">Gerencie suas informações pessoais e segurança da conta.</p>
                    </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-12 gap-6">
                    {/* Left Side: Profile Summary */}
                    <Card className="md:col-span-4 border-none shadow-xl bg-card/60 backdrop-blur-sm h-fit sticky top-6">
                        <CardHeader className="text-center pb-2">
                            <div className="relative group mx-auto mb-4">
                                <Avatar className="size-28 border-4 border-background shadow-2xl mx-auto ring-4 ring-primary/10">
                                    <AvatarImage src={pictureUrl || `https://api.dicebear.com/7.x/avataaars/svg?seed=${user?.userName}`} />
                                    <AvatarFallback className="text-3xl bg-primary/20 text-primary font-bold">
                                        {user?.name?.[0]}{user?.userName?.[0]}
                                    </AvatarFallback>
                                </Avatar>
                                <input
                                    type="file"
                                    ref={fileInputRef}
                                    accept="image/*"
                                    className="hidden"
                                    onChange={(e) => {
                                        const file = e.target.files?.[0];
                                        if (file) uploadPicture(file);
                                    }}
                                />
                                <button
                                    type="button"
                                    onClick={() => fileInputRef.current?.click()}
                                    disabled={isUploading}
                                    className="absolute bottom-1 right-1/2 translate-x-12 p-2 bg-primary text-primary-foreground rounded-full shadow-lg hover:scale-110 transition-transform disabled:opacity-50"
                                >
                                    {isUploading ? <Clock size={16} className="animate-spin" /> : <Camera size={16} />}
                                </button>
                                {pictureUrl && (
                                    <button
                                        type="button"
                                        onClick={deletePicture}
                                        className="absolute bottom-1 right-1/2 -translate-x-5 p-2 bg-destructive text-destructive-foreground rounded-full shadow-lg hover:scale-110 transition-transform"
                                    >
                                        <Trash2 size={16} />
                                    </button>
                                )}
                            </div>
                            <CardTitle className="text-xl font-bold">{user?.name || user?.userName}</CardTitle>
                            <CardDescription className="font-mono text-xs">@{user?.userName}</CardDescription>

                            <div className="flex items-center justify-center mt-3">
                                <Badge variant="secondary" className="bg-emerald-500/10 text-emerald-600 border-emerald-500/20 gap-1.5 py-1 px-3 rounded-full flex items-center">
                                    <CheckCircle2 size={12} />
                                    Conta Verificada
                                </Badge>
                            </div>
                        </CardHeader>
                        <CardContent className="pt-6 space-y-4">
                            <div className="space-y-1">
                                <p className="text-[10px] font-bold text-muted-foreground uppercase">E-mail</p>
                                <p className="text-sm font-medium truncate">{user?.email}</p>
                            </div>
                            <div className="space-y-1">
                                <p className="text-[10px] font-bold text-muted-foreground uppercase">Membro desde</p>
                                <p className="text-sm font-medium">Janeiro de 2024</p>
                            </div>
                        </CardContent>
                    </Card>

                    {/* Right Side: Tabs Content */}
                    <Card className="md:col-span-8 border-none shadow-xl overflow-hidden">
                        <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
                            <div className="px-6 border-b bg-muted/30">
                                <TabsList className="h-14 bg-transparent p-0 gap-6 overflow-x-auto no-scrollbar">
                                    <TabsTrigger value="info" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-primary data-[state=active]:bg-transparent data-[state=active]:shadow-none px-0 text-sm font-bold gap-2 shrink-0">
                                        <User size={16} />
                                        Informações do Usuário
                                    </TabsTrigger>
                                    <TabsTrigger value="security" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-primary data-[state=active]:bg-transparent data-[state=active]:shadow-none px-0 text-sm font-bold gap-2 shrink-0">
                                        <Lock size={16} />
                                        Segurança
                                    </TabsTrigger>
                                    <TabsTrigger value="roles" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-primary data-[state=active]:bg-transparent data-[state=active]:shadow-none px-0 text-sm font-bold gap-2 shrink-0">
                                        <Shield size={16} />
                                        Grupos de permissões (1)
                                    </TabsTrigger>
                                    <TabsTrigger value="org" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-primary data-[state=active]:bg-transparent data-[state=active]:shadow-none px-0 text-sm font-bold gap-2 shrink-0">
                                        <Building2 size={16} />
                                        Unidades organizacionais
                                    </TabsTrigger>
                                </TabsList>
                            </div>

                            <div className="p-0">
                                {/* TAB: Informações */}
                                <TabsContent value="info" className="m-0 focus-visible:ring-0">
                                    <form onSubmit={handleUpdateProfile}>
                                        <div className="p-8 space-y-6">
                                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                                                <div className="space-y-2 sm:col-span-2">
                                                    <Label htmlFor="username" className="text-xs font-bold uppercase text-muted-foreground">Nome do usuário *</Label>
                                                    <Input
                                                        id="username"
                                                        value={userName}
                                                        onChange={e => setUserName(e.target.value)}
                                                        className="h-11 bg-muted/20 border-muted-foreground/20 focus:bg-background transition-all"
                                                        placeholder="admin"
                                                    />
                                                </div>
                                                <div className="space-y-2">
                                                    <Label htmlFor="name" className="text-xs font-bold uppercase text-muted-foreground">Nome</Label>
                                                    <Input
                                                        id="name"
                                                        value={name}
                                                        onChange={setName ? (e => setName(e.target.value)) : undefined}
                                                        className="h-11 bg-muted/20 border-muted-foreground/20 focus:bg-background transition-all"
                                                        placeholder="admin"
                                                    />
                                                </div>
                                                <div className="space-y-2">
                                                    <Label htmlFor="surname" className="text-xs font-bold uppercase text-muted-foreground">Sobrenome</Label>
                                                    <Input
                                                        id="surname"
                                                        value={surName}
                                                        onChange={e => setSurName(e.target.value)}
                                                        className="h-11 bg-muted/20 border-muted-foreground/20 focus:bg-background transition-all"
                                                        placeholder="-"
                                                    />
                                                </div>
                                                <div className="space-y-2 sm:col-span-2">
                                                    <Label htmlFor="email" className="text-xs font-bold uppercase text-muted-foreground">Endereço de e-mail *</Label>
                                                    <div className="relative">
                                                        <Mail className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-muted-foreground" />
                                                        <Input
                                                            id="email"
                                                            type="email"
                                                            value={email}
                                                            onChange={e => setEmail(e.target.value)}
                                                            className="h-11 pl-10 bg-muted/20 border-muted-foreground/20 focus:bg-background transition-all"
                                                            placeholder="admin@abp.io"
                                                        />
                                                    </div>
                                                </div>
                                                <div className="space-y-2 sm:col-span-2">
                                                    <Label htmlFor="phone" className="text-xs font-bold uppercase text-muted-foreground">Número de telefone</Label>
                                                    <div className="relative">
                                                        <Smartphone className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-muted-foreground" />
                                                        <Input
                                                            id="phone"
                                                            value={phoneNumber}
                                                            onChange={e => setPhoneNumber(e.target.value)}
                                                            className="h-11 pl-10 bg-muted/20 border-muted-foreground/20 focus:bg-background transition-all"
                                                            placeholder="(00) 00000-0000"
                                                        />
                                                    </div>
                                                </div>
                                            </div>

                                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 pt-4 border-t border-dashed">
                                                <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-muted/10 transition-colors cursor-pointer">
                                                    <Checkbox id="active" defaultChecked />
                                                    <label htmlFor="active" className="text-sm font-semibold cursor-pointer">Ativo</label>
                                                </div>
                                                <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-muted/10 transition-colors cursor-pointer">
                                                    <Checkbox id="lockout" defaultChecked />
                                                    <div className="flex items-center gap-1.5">
                                                        <label htmlFor="lockout" className="text-sm font-semibold cursor-pointer">Bloquear conta após falhas</label>
                                                        <Info size={14} className="text-muted-foreground" />
                                                    </div>
                                                </div>
                                                <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-muted/10 transition-colors cursor-pointer opacity-50">
                                                    <Checkbox id="email-confirmed" disabled />
                                                    <label htmlFor="email-confirmed" className="text-sm font-semibold cursor-not-allowed">E-mail verificado</label>
                                                </div>
                                                <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-muted/10 transition-colors cursor-pointer">
                                                    <Checkbox id="phone-confirmed" />
                                                    <label htmlFor="phone-confirmed" className="text-sm font-semibold cursor-pointer">Telefone verificado</label>
                                                </div>
                                            </div>
                                        </div>
                                        <CardFooter className="bg-muted/20 border-t p-6 justify-end gap-3 flex-wrap">
                                            <Button variant="ghost" type="button" className="px-6 font-bold text-muted-foreground">Cancelar</Button>
                                            <Button type="submit" disabled={isUpdating} className="px-10 font-bold shadow-lg shadow-primary/20 h-11">
                                                {isUpdating ? <Clock className="animate-spin size-4 mr-2" /> : <Save size={18} className="mr-2" />}
                                                Salvar Perfil
                                            </Button>
                                        </CardFooter>
                                    </form>
                                </TabsContent>

                                {/* TAB: Segurança */}
                                <TabsContent value="security" className="m-0 focus-visible:ring-0">
                                    <div className="p-8 space-y-8">
                                        <div className="space-y-4">
                                            <div className="flex items-center gap-3 text-primary">
                                                <Lock size={20} className="fill-primary/10" />
                                                <h3 className="text-lg font-bold">Alterar Senha</h3>
                                            </div>
                                            <div className="grid grid-cols-1 gap-5">
                                                <div className="space-y-2">
                                                    <Label htmlFor="current-pass" className="text-xs font-bold uppercase text-muted-foreground">Senha atual</Label>
                                                    <Input
                                                        id="current-pass"
                                                        type="password"
                                                        value={currentPassword}
                                                        onChange={e => setCurrentPassword(e.target.value)}
                                                        className="h-11 bg-muted/20 border-muted-foreground/20 focus:bg-background transition-all"
                                                    />
                                                </div>
                                                <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                                                    <div className="space-y-2">
                                                        <Label htmlFor="new-pass" className="text-xs font-bold uppercase text-muted-foreground">Nova senha</Label>
                                                        <Input
                                                            id="new-pass"
                                                            type="password"
                                                            value={newPassword}
                                                            onChange={e => setNewPassword(e.target.value)}
                                                            className="h-11 bg-muted/20 border-muted-foreground/20 focus:bg-background transition-all"
                                                        />
                                                    </div>
                                                    <div className="space-y-2">
                                                        <Label htmlFor="confirm-pass" className="text-xs font-bold uppercase text-muted-foreground">Confirmar nova senha</Label>
                                                        <Input
                                                            id="confirm-pass"
                                                            type="password"
                                                            value={confirmPassword}
                                                            onChange={e => setConfirmPassword(e.target.value)}
                                                            className="h-11 bg-muted/20 border-muted-foreground/20 focus:bg-background transition-all"
                                                        />
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="flex pt-4">
                                                <Button className="font-bold gap-2">
                                                    <Lock size={16} />
                                                    Atualizar Senha
                                                </Button>
                                            </div>
                                        </div>

                                        <div className="space-y-4 pt-8 border-t border-dashed">
                                            <div className="flex items-center gap-3 text-blue-600">
                                                <Fingerprint size={20} />
                                                <h3 className="text-lg font-bold">Autenticação em Dois Fatores</h3>
                                            </div>
                                            <div className="bg-blue-500/5 border border-blue-200 dark:border-blue-900/30 p-4 rounded-xl flex items-center justify-between gap-4">
                                                <div>
                                                    <p className="font-bold text-sm">Adicione uma camada extra de segurança.</p>
                                                    <p className="text-xs text-muted-foreground">Use um aplicativo como Google Authenticator para gerar códigos.</p>
                                                </div>
                                                <Button variant="outline" className="border-blue-200 text-blue-600 hover:bg-blue-500 hover:text-white transition-all">Configurar 2FA</Button>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="p-6 bg-muted/10 border-t flex justify-between items-center px-8">
                                        <p className="text-xs text-muted-foreground italic flex items-center gap-1.5">
                                            <Info size={12} />
                                            Recomendamos trocar sua senha a cada 90 dias.
                                        </p>
                                    </div>
                                </TabsContent>

                                {/* TAB: Funções (Roles) */}
                                <TabsContent value="roles" className="m-0 focus-visible:ring-0">
                                    <div className="p-8 space-y-6">
                                        <div className="flex items-center gap-3 text-primary mb-2">
                                            <ShieldCheck size={20} className="fill-primary/10" />
                                            <h3 className="text-lg font-bold">Grupos de permissões</h3>
                                        </div>
                                        <p className="text-sm text-muted-foreground mb-6">Abaixo estão os grupos de permissões aos quais você pertence.</p>

                                        <div className="space-y-3">
                                            {["admin"].map((role) => (
                                                <div key={role} className="flex items-center justify-between p-4 rounded-xl border bg-muted/5 group hover:bg-emerald-500/5 hover:border-emerald-500/20 transition-all cursor-default">
                                                    <div className="flex items-center gap-4">
                                                        <div className="size-10 rounded-lg bg-background border flex items-center justify-center text-emerald-600 shadow-sm group-hover:scale-110 transition-transform">
                                                            <Shield size={20} />
                                                        </div>
                                                        <div>
                                                            <p className="font-bold text-sm uppercase tracking-wide">{role}</p>
                                                            <p className="text-xs text-muted-foreground">Grupo de permissões padrão do sistema</p>
                                                        </div>
                                                    </div>
                                                    <div className="flex items-center gap-2">
                                                        <Checkbox id={`role-${role}`} checked disabled />
                                                        <label htmlFor={`role-${role}`} className="text-xs font-bold text-emerald-600">ATIVO</label>
                                                    </div>
                                                </div>
                                            ))}
                                        </div>
                                    </div>
                                </TabsContent>

                                {/* TAB: Unidades Organizacionais */}
                                <TabsContent value="org" className="m-0 focus-visible:ring-0">
                                    <div className="p-16 flex flex-col items-center justify-center text-center space-y-4 opacity-70">
                                        <div className="size-20 rounded-full bg-muted flex items-center justify-center border-4 border-background shadow-inner">
                                            <Building2 className="size-10 text-muted-foreground" />
                                        </div>
                                        <p className="text-lg font-bold text-muted-foreground">Sem unidades organizacionais!</p>
                                        <p className="text-sm text-muted-foreground max-w-xs">Você não está vinculado a nenhuma unidade organizacional no momento.</p>
                                    </div>
                                </TabsContent>
                            </div>
                        </Tabs>
                    </Card>
                </div>
            </div>
        </AppShell>
    );
}
