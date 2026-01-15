import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Network, Plus, Trash2, Edit2, Users } from "lucide-react";
import { Button } from "@/components/ui/button";

export default function OrgUnitsPage() {
    return (
        <AppShell>
            <div className="space-y-6">
                <div className="flex items-center justify-between">
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight text-foreground/90">Unidades Organizacionais</h1>
                        <p className="text-muted-foreground">Gerencie a estrutura hierárquica da sua empresa.</p>
                    </div>
                    <Button className="gap-2 shadow-lg shadow-primary/20">
                        <Plus className="size-4" />
                        Nova Unidade Raiz
                    </Button>
                </div>

                <div className="grid md:grid-cols-2 gap-6">
                    <Card className="border-none shadow-sm h-fit">
                        <CardHeader>
                            <CardTitle className="text-lg">Estrutura</CardTitle>
                        </CardHeader>
                        <CardContent>
                            <div className="space-y-2">
                                <div className="flex items-center justify-between p-3 bg-primary/5 rounded-lg border border-primary/10">
                                    <div className="flex items-center gap-3">
                                        <Network className="size-4 text-primary" />
                                        <span className="font-medium">Sede Principal</span>
                                    </div>
                                    <div className="flex items-center gap-1">
                                        <Button variant="ghost" size="icon" className="size-8"><Plus className="size-4" /></Button>
                                        <Button variant="ghost" size="icon" className="size-8 text-primary"><Edit2 className="size-4" /></Button>
                                        <Button variant="ghost" size="icon" className="size-8 text-destructive"><Trash2 className="size-4" /></Button>
                                    </div>
                                </div>
                                <div className="ml-8 flex items-center justify-between p-3 hover:bg-muted/50 rounded-lg transition-colors border border-transparent hover:border-border">
                                    <div className="flex items-center gap-3">
                                        <Network className="size-4 text-muted-foreground" />
                                        <span className="text-sm">Departamento de TI</span>
                                    </div>
                                    <div className="flex items-center gap-1">
                                        <Button variant="ghost" size="icon" className="size-8"><Plus className="size-4" /></Button>
                                        <Button variant="ghost" size="icon" className="size-8"><Edit2 className="size-4" /></Button>
                                        <Button variant="ghost" size="icon" className="size-8 text-destructive"><Trash2 className="size-4" /></Button>
                                    </div>
                                </div>
                                <div className="ml-8 flex items-center justify-between p-3 hover:bg-muted/50 rounded-lg transition-colors border border-transparent hover:border-border">
                                    <div className="flex items-center gap-3">
                                        <Network className="size-4 text-muted-foreground" />
                                        <span className="text-sm">Recursos Humanos</span>
                                    </div>
                                    <div className="flex items-center gap-1">
                                        <Button variant="ghost" size="icon" className="size-8"><Plus className="size-4" /></Button>
                                        <Button variant="ghost" size="icon" className="size-8"><Edit2 className="size-4" /></Button>
                                        <Button variant="ghost" size="icon" className="size-8 text-destructive"><Trash2 className="size-4" /></Button>
                                    </div>
                                </div>
                            </div>
                        </CardContent>
                    </Card>

                    <Card className="border-none shadow-sm">
                        <CardHeader>
                            <div className="flex items-center justify-between">
                                <CardTitle className="text-lg">Membros da Unidade</CardTitle>
                                <Button variant="outline" size="sm" className="gap-2">
                                    <Users className="size-4" />
                                    Adicionar Usuário
                                </Button>
                            </div>
                        </CardHeader>
                        <CardContent className="h-[300px] flex flex-col items-center justify-center text-center opacity-50 grayscale">
                            <Users className="size-12 mb-4" />
                            <p className="text-sm">Selecione uma unidade para ver seus membros.</p>
                        </CardContent>
                    </Card>
                </div>
            </div>
        </AppShell>
    );
}
