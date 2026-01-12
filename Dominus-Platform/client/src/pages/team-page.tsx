import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams } from "wouter";
import { useAuth } from "@/lib/auth-store";
import { api } from "@/lib/api";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Badge } from "@/components/ui/badge";
import { Separator } from "@/components/ui/separator";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { AlertDialog, AlertDialogAction, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger } from "@/components/ui/alert-dialog";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Checkbox } from "@/components/ui/checkbox";
import { AppShell } from "@/components/layout/shell";
import { toast } from "sonner";
import { Users, UserPlus, Mail, Trash2, Settings, Clock, CheckCircle, XCircle, Shield } from "lucide-react";
import { 
  ROLE_LABELS, 
  MODULE_LABELS, 
  ACTION_LABELS, 
  DEFAULT_ROLE_PERMISSIONS,
  ALL_MODULES,
  ALL_ACTIONS,
  type MemberRole,
  type PermissionModule,
  type PermissionAction 
} from "@shared/schema";

export default function TeamPage() {
  const { slug } = useParams<{ slug: string }>();
  const currentWorkspace = useAuth((s) => s.currentWorkspace);
  const user = useAuth((s) => s.user);
  const queryClient = useQueryClient();
  
  const [inviteEmail, setInviteEmail] = useState("");
  const [inviteRole, setInviteRole] = useState<MemberRole>("MEMBRO");
  const [inviteDialogOpen, setInviteDialogOpen] = useState(false);
  const [permissionsDialogOpen, setPermissionsDialogOpen] = useState(false);
  const [selectedMember, setSelectedMember] = useState<any>(null);

  const { data: members = [], isLoading: membersLoading } = useQuery({
    queryKey: ["team", currentWorkspace?.id],
    queryFn: () => api.getTeamMembers(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: invites = [] } = useQuery({
    queryKey: ["invites", currentWorkspace?.id],
    queryFn: () => api.getWorkspaceInvites(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: memberPermissions = [] } = useQuery({
    queryKey: ["member-permissions", selectedMember?.id],
    queryFn: () => api.getMemberPermissions(currentWorkspace!.id, selectedMember!.id),
    enabled: !!selectedMember && !!currentWorkspace,
  });

  const inviteMutation = useMutation({
    mutationFn: () => api.createInvite(currentWorkspace!.id, inviteEmail, inviteRole),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["invites"] });
      setInviteEmail("");
      setInviteRole("MEMBRO");
      setInviteDialogOpen(false);
      toast.success("Convite enviado com sucesso!");
    },
    onError: (error: any) => {
      toast.error(error.message || "Erro ao enviar convite");
    },
  });

  const updateRoleMutation = useMutation({
    mutationFn: ({ memberId, memberRole }: { memberId: string; memberRole: string }) =>
      api.updateMemberRole(currentWorkspace!.id, memberId, memberRole),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["team"] });
      toast.success("Papel atualizado com sucesso!");
    },
    onError: (error: any) => {
      toast.error(error.message || "Erro ao atualizar papel");
    },
  });

  const removeMemberMutation = useMutation({
    mutationFn: (memberId: string) => api.removeMember(currentWorkspace!.id, memberId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["team"] });
      toast.success("Membro removido com sucesso!");
    },
    onError: (error: any) => {
      toast.error(error.message || "Erro ao remover membro");
    },
  });

  const cancelInviteMutation = useMutation({
    mutationFn: (inviteId: string) => api.cancelInvite(currentWorkspace!.id, inviteId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["invites"] });
      toast.success("Convite cancelado!");
    },
  });

  const setPermissionMutation = useMutation({
    mutationFn: ({ module, action, granted }: { module: string; action: string; granted: boolean }) =>
      api.setMemberPermission(currentWorkspace!.id, selectedMember!.id, module, action, granted),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["member-permissions", selectedMember?.id] });
    },
  });

  const getInitials = (name: string) => {
    return name
      .split(" ")
      .map((n) => n[0])
      .join("")
      .toUpperCase()
      .slice(0, 2);
  };

  const getRoleBadgeColor = (role: MemberRole) => {
    const colors: Record<MemberRole, string> = {
      ADMIN: "bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200",
      GERENTE: "bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200",
      VENDEDOR: "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200",
      FINANCEIRO: "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200",
      MEMBRO: "bg-gray-100 text-gray-800 dark:bg-gray-900 dark:text-gray-200",
    };
    return colors[role] || colors.MEMBRO;
  };

  const hasDefaultPermission = (role: MemberRole, module: PermissionModule, action: PermissionAction) => {
    return DEFAULT_ROLE_PERMISSIONS[role]?.[module]?.includes(action) ?? false;
  };

  const hasExtraPermission = (module: PermissionModule, action: PermissionAction) => {
    return memberPermissions.some(
      (p: any) => p.module === module && p.action === action && p.granted
    );
  };

  const isPermissionRevoked = (module: PermissionModule, action: PermissionAction) => {
    return memberPermissions.some(
      (p: any) => p.module === module && p.action === action && !p.granted
    );
  };

  const handlePermissionChange = (module: PermissionModule, action: PermissionAction, checked: boolean) => {
    setPermissionMutation.mutate({ module, action, granted: checked });
  };

  const isOwner = (member: any) => member.role === "OWNER";
  const isCurrentUser = (member: any) => member.userId === user?.id;

  return (
    <AppShell>
      <div className="container max-w-5xl py-8">
        <div className="flex items-center justify-between mb-8">
          <div>
            <h1 className="text-3xl font-bold" data-testid="text-page-title">Equipe</h1>
            <p className="text-muted-foreground mt-1">Gerencie os membros do seu workspace</p>
          </div>
          <Dialog open={inviteDialogOpen} onOpenChange={setInviteDialogOpen}>
            <DialogTrigger asChild>
              <Button data-testid="button-invite-member">
                <UserPlus className="mr-2 h-4 w-4" />
                Convidar Membro
              </Button>
            </DialogTrigger>
            <DialogContent>
              <DialogHeader>
                <DialogTitle>Convidar Novo Membro</DialogTitle>
                <DialogDescription>
                  Envie um convite por email para adicionar um novo membro ao workspace.
                </DialogDescription>
              </DialogHeader>
              <div className="space-y-4 py-4">
                <div className="space-y-2">
                  <Label htmlFor="email">Email</Label>
                  <Input
                    id="email"
                    data-testid="input-invite-email"
                    type="email"
                    value={inviteEmail}
                    onChange={(e) => setInviteEmail(e.target.value)}
                    placeholder="email@exemplo.com"
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="role">Papel</Label>
                  <Select value={inviteRole} onValueChange={(v) => setInviteRole(v as MemberRole)}>
                    <SelectTrigger data-testid="select-invite-role">
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      {Object.entries(ROLE_LABELS).map(([value, label]) => (
                        <SelectItem key={value} value={value}>{label}</SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
              </div>
              <DialogFooter>
                <Button variant="outline" onClick={() => setInviteDialogOpen(false)}>
                  Cancelar
                </Button>
                <Button 
                  data-testid="button-send-invite"
                  onClick={() => inviteMutation.mutate()}
                  disabled={!inviteEmail || inviteMutation.isPending}
                >
                  {inviteMutation.isPending ? "Enviando..." : "Enviar Convite"}
                </Button>
              </DialogFooter>
            </DialogContent>
          </Dialog>
        </div>

        <Tabs defaultValue="members">
          <TabsList className="mb-4">
            <TabsTrigger value="members" data-testid="tab-members">
              <Users className="mr-2 h-4 w-4" />
              Membros ({members.length})
            </TabsTrigger>
            <TabsTrigger value="invites" data-testid="tab-invites">
              <Mail className="mr-2 h-4 w-4" />
              Convites ({invites.filter((i: any) => i.status === "PENDING").length})
            </TabsTrigger>
          </TabsList>

          <TabsContent value="members">
            <Card>
              <CardContent className="p-0">
                <Table>
                  <TableHeader>
                    <TableRow>
                      <TableHead>Membro</TableHead>
                      <TableHead>Papel</TableHead>
                      <TableHead>Desde</TableHead>
                      <TableHead className="text-right">Ações</TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {members.map((member: any) => (
                      <TableRow key={member.id} data-testid={`row-member-${member.id}`}>
                        <TableCell>
                          <div className="flex items-center gap-3">
                            <Avatar>
                              <AvatarImage src={member.user.avatar} />
                              <AvatarFallback>{getInitials(member.user.name)}</AvatarFallback>
                            </Avatar>
                            <div>
                              <p className="font-medium">
                                {member.user.name}
                                {isCurrentUser(member) && (
                                  <span className="text-muted-foreground text-sm ml-2">(você)</span>
                                )}
                              </p>
                              <p className="text-sm text-muted-foreground">{member.user.email}</p>
                            </div>
                          </div>
                        </TableCell>
                        <TableCell>
                          {isOwner(member) ? (
                            <Badge variant="outline" className="bg-primary/10">
                              <Shield className="mr-1 h-3 w-3" />
                              Proprietário
                            </Badge>
                          ) : (
                            <Select
                              value={member.memberRole}
                              onValueChange={(value) => updateRoleMutation.mutate({ memberId: member.id, memberRole: value })}
                              disabled={isCurrentUser(member)}
                            >
                              <SelectTrigger className="w-[140px]">
                                <SelectValue />
                              </SelectTrigger>
                              <SelectContent>
                                {Object.entries(ROLE_LABELS).map(([value, label]) => (
                                  <SelectItem key={value} value={value}>{label}</SelectItem>
                                ))}
                              </SelectContent>
                            </Select>
                          )}
                        </TableCell>
                        <TableCell className="text-muted-foreground">
                          {new Date(member.createdAt).toLocaleDateString("pt-BR")}
                        </TableCell>
                        <TableCell className="text-right">
                          <div className="flex items-center justify-end gap-2">
                            {!isOwner(member) && (
                              <Button
                                variant="ghost"
                                size="icon"
                                data-testid={`button-permissions-${member.id}`}
                                onClick={() => {
                                  setSelectedMember(member);
                                  setPermissionsDialogOpen(true);
                                }}
                              >
                                <Settings className="h-4 w-4" />
                              </Button>
                            )}
                            {!isOwner(member) && !isCurrentUser(member) && (
                              <AlertDialog>
                                <AlertDialogTrigger asChild>
                                  <Button variant="ghost" size="icon" className="text-destructive">
                                    <Trash2 className="h-4 w-4" />
                                  </Button>
                                </AlertDialogTrigger>
                                <AlertDialogContent>
                                  <AlertDialogHeader>
                                    <AlertDialogTitle>Remover Membro?</AlertDialogTitle>
                                    <AlertDialogDescription>
                                      Tem certeza que deseja remover {member.user.name} do workspace? Esta ação não pode ser desfeita.
                                    </AlertDialogDescription>
                                  </AlertDialogHeader>
                                  <AlertDialogFooter>
                                    <AlertDialogCancel>Cancelar</AlertDialogCancel>
                                    <AlertDialogAction
                                      onClick={() => removeMemberMutation.mutate(member.id)}
                                      className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
                                    >
                                      Remover
                                    </AlertDialogAction>
                                  </AlertDialogFooter>
                                </AlertDialogContent>
                              </AlertDialog>
                            )}
                          </div>
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </CardContent>
            </Card>
          </TabsContent>

          <TabsContent value="invites">
            <Card>
              <CardContent className="p-0">
                {invites.length === 0 ? (
                  <div className="p-8 text-center text-muted-foreground">
                    Nenhum convite pendente
                  </div>
                ) : (
                  <Table>
                    <TableHeader>
                      <TableRow>
                        <TableHead>Email</TableHead>
                        <TableHead>Papel</TableHead>
                        <TableHead>Status</TableHead>
                        <TableHead>Expira em</TableHead>
                        <TableHead className="text-right">Ações</TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {invites.map((invite: any) => (
                        <TableRow key={invite.id} data-testid={`row-invite-${invite.id}`}>
                          <TableCell>{invite.email}</TableCell>
                          <TableCell>
                            <Badge className={getRoleBadgeColor(invite.memberRole)}>
                              {ROLE_LABELS[invite.memberRole as MemberRole]}
                            </Badge>
                          </TableCell>
                          <TableCell>
                            {invite.status === "PENDING" && (
                              <Badge variant="outline" className="bg-yellow-50 text-yellow-700 border-yellow-200">
                                <Clock className="mr-1 h-3 w-3" />
                                Pendente
                              </Badge>
                            )}
                            {invite.status === "ACCEPTED" && (
                              <Badge variant="outline" className="bg-green-50 text-green-700 border-green-200">
                                <CheckCircle className="mr-1 h-3 w-3" />
                                Aceito
                              </Badge>
                            )}
                            {invite.status === "EXPIRED" && (
                              <Badge variant="outline" className="bg-gray-50 text-gray-700 border-gray-200">
                                <XCircle className="mr-1 h-3 w-3" />
                                Expirado
                              </Badge>
                            )}
                          </TableCell>
                          <TableCell className="text-muted-foreground">
                            {new Date(invite.expiresAt).toLocaleDateString("pt-BR")}
                          </TableCell>
                          <TableCell className="text-right">
                            {invite.status === "PENDING" && (
                              <Button
                                variant="ghost"
                                size="sm"
                                onClick={() => cancelInviteMutation.mutate(invite.id)}
                                className="text-destructive"
                              >
                                Cancelar
                              </Button>
                            )}
                          </TableCell>
                        </TableRow>
                      ))}
                    </TableBody>
                  </Table>
                )}
              </CardContent>
            </Card>
          </TabsContent>
        </Tabs>

        <Dialog open={permissionsDialogOpen} onOpenChange={setPermissionsDialogOpen}>
          <DialogContent className="max-w-2xl max-h-[80vh] overflow-y-auto">
            <DialogHeader>
              <DialogTitle>Permissões de {selectedMember?.user?.name}</DialogTitle>
              <DialogDescription>
                Papel: {ROLE_LABELS[selectedMember?.memberRole as MemberRole]}. Configure permissões extras além do padrão do papel.
              </DialogDescription>
            </DialogHeader>
            <div className="py-4">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Módulo</TableHead>
                    {ALL_ACTIONS.map((action) => (
                      <TableHead key={action} className="text-center w-24">
                        {ACTION_LABELS[action]}
                      </TableHead>
                    ))}
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {ALL_MODULES.map((module) => (
                    <TableRow key={module}>
                      <TableCell className="font-medium">{MODULE_LABELS[module]}</TableCell>
                      {ALL_ACTIONS.map((action) => {
                        const hasDefault = selectedMember ? hasDefaultPermission(selectedMember.memberRole, module, action) : false;
                        const hasExtra = hasExtraPermission(module, action);
                        const isRevoked = isPermissionRevoked(module, action);
                        const effectivePermission = isRevoked ? false : (hasDefault || hasExtra);

                        return (
                          <TableCell key={action} className="text-center">
                            <Checkbox
                              checked={effectivePermission}
                              onCheckedChange={(checked) => handlePermissionChange(module, action, checked as boolean)}
                              disabled={setPermissionMutation.isPending}
                            />
                          </TableCell>
                        );
                      })}
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </div>
            <DialogFooter>
              <Button onClick={() => setPermissionsDialogOpen(false)}>Fechar</Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </div>
    </AppShell>
  );
}
