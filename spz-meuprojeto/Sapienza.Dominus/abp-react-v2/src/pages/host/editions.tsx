import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";
import { Separator } from "@/components/ui/separator";
import { Crown, Plus, Settings, MoreHorizontal, Check, X } from "lucide-react";

// Mock data for editions
const mockEditions = [
    {
        id: "1",
        name: "Standard",
        displayName: "Standard Edition",
        price: 0,
        isFree: true,
        features: {
            "MaxUserCount": "10",
            "StorageSize": "1GB",
            "Support": "Email",
        },
    },
    {
        id: "2",
        name: "Professional",
        displayName: "Professional Edition",
        price: 49,
        isFree: false,
        features: {
            "MaxUserCount": "50",
            "StorageSize": "10GB",
            "Support": "24/7 Priority",
        },
    },
    {
        id: "3",
        name: "Enterprise",
        displayName: "Enterprise Edition",
        price: 199,
        isFree: false,
        features: {
            "MaxUserCount": "Unlimited",
            "StorageSize": "100GB",
            "Support": "Dedicated Account Manager",
        },
    },
];

export default function HostEditionsPage() {
    return (
        <AppShell>
            <div className="space-y-6">
                {/* Page Header */}
                <div className="flex items-center gap-3">
                    <Crown className="h-8 w-8 text-primary" />
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">Editions</h1>
                        <p className="text-muted-foreground">
                            Manage subscription plans and features
                        </p>
                    </div>
                </div>

                {/* Editions Table */}
                <Card>
                    <CardHeader>
                        <div className="flex items-center justify-between">
                            <CardTitle>Editions ({mockEditions.length})</CardTitle>
                            <Button className="gap-2">
                                <Plus className="size-4" />
                                New Edition
                            </Button>
                        </div>
                    </CardHeader>
                    <CardContent>
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead>Edition</TableHead>
                                    <TableHead>Pricing</TableHead>
                                    <TableHead>Key Features</TableHead>
                                    <TableHead className="text-right">Actions</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {mockEditions.map((edition) => (
                                    <TableRow key={edition.id}>
                                        <TableCell>
                                            <div>
                                                <p className="font-medium">{edition.displayName}</p>
                                                <p className="text-xs text-muted-foreground">{edition.name}</p>
                                            </div>
                                        </TableCell>
                                        <TableCell>
                                            {edition.isFree ? (
                                                <Badge variant="secondary">Free</Badge>
                                            ) : (
                                                <span className="font-semibold">${edition.price}/mo</span>
                                            )}
                                        </TableCell>
                                        <TableCell>
                                            <div className="flex flex-col gap-1">
                                                {Object.entries(edition.features).map(([key, value]) => (
                                                    <div key={key} className="flex items-center gap-2 text-xs">
                                                        <Check className="h-3 w-3 text-emerald-500" />
                                                        <span className="text-muted-foreground">{key}:</span>
                                                        <span className="font-medium">{value}</span>
                                                    </div>
                                                ))}
                                            </div>
                                        </TableCell>
                                        <TableCell className="text-right">
                                            <div className="flex items-center justify-end gap-2">
                                                <Button variant="ghost" size="icon">
                                                    <Settings className="h-4 w-4" />
                                                </Button>
                                                <Button variant="ghost" size="icon">
                                                    <MoreHorizontal className="h-4 w-4" />
                                                </Button>
                                            </div>
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </CardContent>
                </Card>

                {/* Features Comparison Preview */}
                <div className="grid gap-6 md:grid-cols-3">
                    {mockEditions.map((edition) => (
                        <Card key={edition.id} className={edition.name === "Professional" ? "border-primary shadow-md" : ""}>
                            <CardHeader>
                                <CardTitle>{edition.displayName}</CardTitle>
                                <CardDescription>
                                    {edition.isFree ? "Perfect to get started" : `Starting at $${edition.price}/mo`}
                                </CardDescription>
                            </CardHeader>
                            <CardContent className="space-y-4">
                                <div className="text-3xl font-bold">
                                    {edition.isFree ? "Free" : `$${edition.price}`}
                                    {!edition.isFree && <span className="text-sm font-normal text-muted-foreground">/mo</span>}
                                </div>
                                <Separator />
                                <ul className="space-y-2">
                                    {Object.entries(edition.features).map(([key, value]) => (
                                        <li key={key} className="flex items-center gap-2 text-sm">
                                            <Check className="h-4 w-4 text-emerald-500" />
                                            <span>{key}: {value}</span>
                                        </li>
                                    ))}
                                    <li className="flex items-center gap-2 text-sm text-muted-foreground">
                                        <Check className="h-4 w-4 text-emerald-500" />
                                        <span>Basic Analytics</span>
                                    </li>
                                    {edition.name === "Standard" && (
                                        <li className="flex items-center gap-2 text-sm text-muted-foreground opacity-50">
                                            <X className="h-4 w-4 text-destructive" />
                                            <span>Custom Domain</span>
                                        </li>
                                    )}
                                </ul>
                                <Button className="w-full" variant={edition.name === "Professional" ? "default" : "outline"}>
                                    {edition.isFree ? "Select Plan" : "Upgrade"}
                                </Button>
                            </CardContent>
                        </Card>
                    ))}
                </div>
            </div>
        </AppShell>
    );
}
