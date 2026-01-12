import { useState } from "react";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Search, Loader2 } from "lucide-react";
import { useClients } from "@/lib/abp/hooks/useClients";
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";

interface ClientPickerDialogProps {
    onSelect: (client: any) => void;
    trigger?: React.ReactNode;
}

export function ClientPickerDialog({ onSelect, trigger }: ClientPickerDialogProps) {
    const [open, setOpen] = useState(false);
    const [search, setSearch] = useState("");
    const { data, isLoading } = useClients({ filter: search, maxResultCount: 10 });

    const handleSelect = (client: any) => {
        onSelect(client);
        setOpen(false);
    };

    return (
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogTrigger asChild>
                {trigger || <Button variant="outline">Select Client</Button>}
            </DialogTrigger>
            <DialogContent className="sm:max-w-[700px]">
                <DialogHeader>
                    <DialogTitle>Select Client</DialogTitle>
                </DialogHeader>
                <div className="space-y-4 py-4">
                    <div className="flex items-center space-x-2">
                        <Search className="h-4 w-4 text-muted-foreground" />
                        <Input
                            placeholder="Search clients..."
                            value={search}
                            onChange={(e) => setSearch(e.target.value)}
                            className="flex-1"
                        />
                    </div>

                    <div className="rounded-md border h-[400px] overflow-auto">
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead>Name</TableHead>
                                    <TableHead>Email</TableHead>
                                    <TableHead>CPF/CNPJ</TableHead>
                                    <TableHead className="w-[100px]"></TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {isLoading ? (
                                    <TableRow>
                                        <TableCell colSpan={4} className="h-24 text-center">
                                            <Loader2 className="h-6 w-6 animate-spin mx-auto" />
                                        </TableCell>
                                    </TableRow>
                                ) : data?.items?.length === 0 ? (
                                    <TableRow>
                                        <TableCell colSpan={4} className="h-24 text-center">
                                            No clients found.
                                        </TableCell>
                                    </TableRow>
                                ) : (
                                    data?.items?.map((client: any) => (
                                        <TableRow key={client.id} className="cursor-pointer hover:bg-muted/50" onClick={() => handleSelect(client)}>
                                            <TableCell className="font-medium">{client.name}</TableCell>
                                            <TableCell>{client.email}</TableCell>
                                            <TableCell>{client.cpfCnpj}</TableCell>
                                            <TableCell>
                                                <Button size="sm" variant="ghost" onClick={(e) => {
                                                    e.stopPropagation();
                                                    handleSelect(client);
                                                }}>Select</Button>
                                            </TableCell>
                                        </TableRow>
                                    ))
                                )}
                            </TableBody>
                        </Table>
                    </div>
                </div>
            </DialogContent>
        </Dialog>
    );
}
