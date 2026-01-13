import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useLocation, useParams } from "wouter";
import { Shell } from "@/components/layout/shell";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { Textarea } from "@/components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { ArrowLeft, Save, Loader2, Plus, Trash2 } from "lucide-react";
import { toast } from "sonner";
import { 
  usePropostalItem, 
  useCreatePropostalItem, 
  useUpdatePropostalItem 
} from "@/lib/abp/hooks/usePropostalItems";


import { useProposals } from "@/lib/abp/hooks/useProposals";


const formSchema = z.object({
  
  desc: z.any(),
  
  quant: z.any(),
  
  unitPrice: z.any(),
  
  total: z.any(),
  
  proposalId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;



export default function PropostalItemFormPage() {
  const [, setLocation] = useLocation();
  const { id } = useParams<{ id: string }>();
  const isEditing = !!id;

  const { data: existing, isLoading: loadingExisting } = usePropostalItem(id || "");
  const createMutation = useCreatePropostalItem();
  const updateMutation = useUpdatePropostalItem();

  
  
  const { data: proposals } = useProposals({ maxResultCount: 1000 });
  

  

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

  useEffect(() => {
    if (existing) {
      reset(existing);
      
    }
  }, [existing, reset]);

  

  const onSubmit = async (data: FormValues) => {
    try {
      const payload = {
        ...data,
        
      };

      if (isEditing) {
        await updateMutation.mutateAsync({ id: id!, data: payload });
        toast.success("PropostalItem updated successfully");
      } else {
        await createMutation.mutateAsync(payload);
        toast.success("PropostalItem created successfully");
      }
      setLocation("/admin/propostal-item");
    } catch (error: any) {
      toast.error(error.message || "Failed to save propostalitem");
    }
  };

  const isLoading = createMutation.isPending || updateMutation.isPending;

  if (loadingExisting && isEditing) {
    return (
      <Shell>
        <div className="flex items-center justify-center h-64">
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
              onClick={() => setLocation("/admin/propostal-item")}
            >
              <ArrowLeft className="h-5 w-5" />
            </Button>
            <div>
              <h1 className="text-2xl font-bold tracking-tight">
                {isEditing ? "Edit PropostalItem" : "New PropostalItem"}
              </h1>
              <p className="text-muted-foreground">
                {isEditing ? "Update the details" : "Create a new propostalitem"}
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
                      <Label htmlFor="desc">
                        Desc
                      </Label>
                      
                      <Input 
                        id="desc" 
                        placeholder=""
                        {...register("desc")} 
                      />
                      
                    </div>
                    
                    
                    <div className="space-y-2">
                      <Label htmlFor="quant">
                        Quant
                      </Label>
                      
                      <Input id="quant" type="number" step="any" {...register("quant")} />
                      
                    </div>
                    
                    
                    <div className="space-y-2">
                      <Label htmlFor="unitPrice">
                        UnitPrice
                      </Label>
                      
                      <Input id="unitPrice" type="number" step="any" {...register("unitPrice")} />
                      
                    </div>
                    
                    
                    <div className="space-y-2">
                      <Label htmlFor="total">
                        Total
                      </Label>
                      
                      <Input id="total" type="number" step="any" {...register("total")} />
                      
                    </div>
                    
                    
                    <div className="space-y-2">
                      <Label htmlFor="proposalId">
                        Proposal
                      </Label>
                      
                      <Select
                        value={watch("proposalId")?.toString()}
                        onValueChange={(val) => setValue("proposalId", val)}
                      >
                        <SelectTrigger>
                          <SelectValue placeholder="Select Number" />
                        </SelectTrigger>
                        <SelectContent>
                          { (proposals as any)?.items?.map((item: any) => (
                            <SelectItem key={item.id} value={item.id}>
                              {item.Number}
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                      
                    </div>
                    
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
                    {isEditing ? "Save Changes" : "Create PropostalItem"}
                  </Button>
                  <Button 
                    type="button" 
                    variant="outline" 
                    className="w-full"
                    onClick={() => setLocation("/admin/propostal-item")}
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
