
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useLocation, useRoute } from "wouter";
import { Shell } from "@/components/layout/shell";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { Textarea } from "@/components/ui/textarea";
import { ArrowLeft, Save, Loader2, Plus, Trash2, Search } from "lucide-react";
import { toast } from "sonner";
import {
  useProposal,
  useCreateProposal,
  useUpdateProposal
} from "@/lib/abp/hooks/useProposals";
import { ClientPickerDialog } from "@/components/client/ClientPickerDialog";
import { useClient } from "@/lib/abp/hooks/useClients"; // Import useClient

const formSchema = z.object({

  number: z.string().optional(),

  date: z.string().optional(),

  validate: z.string().optional(),

  obs: z.string().optional(),

  clientId: z.string().optional(),

});

type FormValues = z.infer<typeof formSchema>;


interface PropostalItemItem {
  id?: string;
  desc?: string;
  quant?: number;
  unitPrice?: number;
  total?: number;
  proposalId?: string;
}


export default function ProposalFormPage() {
  const [, setLocation] = useLocation();
  const [match, params] = useRoute("/admin/proposal/:id/edit");
  const id = match ? params?.id : undefined;
  const isEditing = !!id;

  const { data: existing, isLoading: loadingExisting } = useProposal(id || "");
  const createMutation = useCreateProposal();
  const updateMutation = useUpdateProposal();


  const [propostalItemItems, setPropostalItemItems] = useState<PropostalItemItem[]>([]);


  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: {},
  });

  // Watch clientId to fetch client details for display
  const watchedClientId = watch("clientId");
  const { data: selectedClientData } = useClient(watchedClientId || "");

  useEffect(() => {
    if (existing) {
      reset(existing);

      if (existing.propostalItems) {
        setPropostalItemItems(existing.propostalItems);
      }

    }
  }, [existing, reset]);


  const addPropostalItem = () => {
    setPropostalItemItems([...propostalItemItems, {

      desc: "",

      quant: 0,

      unitPrice: 0,

      total: 0,

      proposalId: "",

    }]);
  };

  const removePropostalItem = (index: number) => {
    setPropostalItemItems(propostalItemItems.filter((_, i) => i !== index));
  };

  const updatePropostalItem = (index: number, field: keyof PropostalItemItem, value: any) => {
    const updated = [...propostalItemItems];
    updated[index] = { ...updated[index], [field]: value };
    setPropostalItemItems(updated);
  };


  const onSubmit = async (data: FormValues) => {
    try {
      const payload = {
        ...data,

        propostalItems: propostalItemItems.filter(item =>

          true
        ),

      };

      if (isEditing) {
        await updateMutation.mutateAsync({ id: id!, data: payload });
        toast.success("Proposal updated successfully");
      } else {
        await createMutation.mutateAsync(payload);
        toast.success("Proposal created successfully");
      }
      setLocation("/admin/proposal");
    } catch (error: any) {
      toast.error(error.message || "Failed to save proposal");
    }
  };

  const isLoading = createMutation.isPending || updateMutation.isPending;

  if (loadingExisting && isEditing) {
    return (
      <Shell>
        <div className="flex items-center justify-center p-12">
          <Loader2 className="h-8 w-8 animate-spin text-primary" />
        </div>
      </Shell>
    );
  }

  return (
    <Shell>
      <div className="space-y-6">
        {/* Header */}
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-4">
            <Button
              variant="ghost"
              size="icon"
              onClick={() => setLocation("/admin/proposal")}
            >
              <ArrowLeft className="h-5 w-5" />
            </Button>
            <div>
              <h1 className="text-2xl font-bold tracking-tight">
                {isEditing ? "Edit Proposal" : "New Proposal"}
              </h1>
              <p className="text-muted-foreground">
                {isEditing ? "Update the details" : "Create a new proposal"}
              </p>
            </div>
          </div>
        </div>

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
            {/* Main Content - 2 columns */}
            <div className="lg:col-span-2 space-y-6">
              {/* Basic Information Card */}
              <Card>
                <CardHeader>
                  <CardTitle>Basic Information</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-4">

                    <div className="space-y-2">
                      <Label htmlFor="number">
                        Number
                      </Label>

                      <Input
                        id="number"
                        placeholder=""
                        {...register("number")}
                      />

                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="date">
                        Date
                      </Label>

                      <Input id="date" type="date" {...register("date")} />

                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="validate">
                        Validate
                      </Label>

                      <Input id="validate" type="date" {...register("validate")} />

                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="obs">
                        Obs
                      </Label>

                      <Textarea
                        id="obs"
                        rows={4}
                        placeholder=""
                        {...register("obs")}
                      />

                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="clientId">Client</Label>
                      <div className="flex gap-2">
                        <Input
                          id="clientName"
                          value={selectedClientData?.name || watchedClientId || ""}
                          readOnly
                          className="bg-muted cursor-default"
                          placeholder="No client selected"
                        />
                        <Input
                          type="hidden"
                          {...register("clientId")}
                        />
                        <ClientPickerDialog
                          onSelect={(client) => setValue("clientId", client.id)}
                        />
                      </div>
                    </div>

                  </div>
                </CardContent>
              </Card>


              {/* Child Entity Card */}
              <Card>
                <CardHeader className="flex flex-row items-center justify-between">
                  <CardTitle>PropostalItem Items</CardTitle>
                  <Button type="button" variant="outline" size="sm" onClick={addPropostalItem}>
                    <Plus className="h-4 w-4 mr-1" />
                    Add Item
                  </Button>
                </CardHeader>
                <CardContent>
                  <div className="space-y-3">
                    {propostalItemItems.map((item, index) => (
                      <div key={index} className="p-4 border rounded-lg space-y-3 bg-muted/30">
                        <div className="flex gap-3 items-start">
                          <div className="flex-1 grid grid-cols-2 md:grid-cols-5 gap-3">

                            <div>
                              <Label className="text-xs text-muted-foreground">Desc</Label>

                              <Input
                                value={item.desc || ""}
                                onChange={(e) => updatePropostalItem(index, "desc", e.target.value)}
                              />

                            </div>

                            <div>
                              <Label className="text-xs text-muted-foreground">Quant</Label>

                              <Input
                                type="number"
                                step="any"
                                value={item.quant || ""}
                                onChange={(e) => updatePropostalItem(index, "quant", e.target.value)}
                              />

                            </div>

                            <div>
                              <Label className="text-xs text-muted-foreground">UnitPrice</Label>

                              <Input
                                type="number"
                                step="any"
                                value={item.unitPrice || ""}
                                onChange={(e) => updatePropostalItem(index, "unitPrice", e.target.value)}
                              />

                            </div>

                            <div>
                              <Label className="text-xs text-muted-foreground">Total</Label>

                              <Input
                                type="number"
                                step="any"
                                value={item.total || ""}
                                onChange={(e) => updatePropostalItem(index, "total", e.target.value)}
                              />

                            </div>

                          </div>
                          {propostalItemItems.length > 1 && (
                            <Button
                              type="button"
                              variant="ghost"
                              size="icon"
                              onClick={() => removePropostalItem(index)}
                            >
                              <Trash2 className="h-4 w-4 text-destructive" />
                            </Button>
                          )}
                        </div>
                      </div>
                    ))}
                    {propostalItemItems.length === 0 && (
                      <div className="text-center py-8 text-muted-foreground">
                        No items yet. Click "Add Item" to start.
                      </div>
                    )}
                  </div>
                </CardContent>
              </Card>

            </div>

            {/* Sidebar - 1 column */}
            <div className="space-y-6">
              <Card className="sticky top-6">
                <CardHeader>
                  <CardTitle>Actions</CardTitle>
                </CardHeader>
                <CardContent className="space-y-3">
                  <Button type="submit" className="w-full" disabled={isLoading}>
                    {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                    <Save className="mr-2 h-4 w-4" />
                    {isEditing ? "Save Changes" : "Create Proposal"}
                  </Button>
                  <Button
                    type="button"
                    variant="outline"
                    className="w-full"
                    onClick={() => setLocation("/admin/proposal")}
                  >
                    Cancel
                  </Button>
                </CardContent>
              </Card>
            </div>
          </div>
        </form>
      </div>
    </Shell>
  );
}
