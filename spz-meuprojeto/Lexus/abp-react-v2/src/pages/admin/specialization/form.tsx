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
  useSpecialization, 
  useCreateSpecialization, 
  useUpdateSpecialization 
} from "@/lib/abp/hooks/useSpecializations";

import { useLawyerSpecializations } from "@/lib/abp/hooks/useLawyerSpecializations";



const formSchema = z.object({
  
  name: z.any(),
  
  description: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;


interface LawyerSpecializationItem {
  id?: string;
  
  lawyerId: ;
  
  specializationId: ;
  
}


export default function SpecializationFormPage() {
  const [, setLocation] = useLocation();
  const { id } = useParams<{ id: string }>();
  const isEditing = !!id;

  const { data: existing, isLoading: loadingExisting } = useSpecialization(id || "");
  const createMutation = useCreateSpecialization();
  const updateMutation = useUpdateSpecialization();

  
  const { data: lawyerSpecializations } = useLawyerSpecializations({ maxResultCount: 1000 });
  
  

  
  const [lawyerSpecializationItems, setLawyerSpecializationItems] = useState<LawyerSpecializationItem[]>([]);
  

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
      
      if (existing.lawyerSpecializations) {
        setLawyerSpecializationItems(existing.lawyerSpecializations);
      }
      
    }
  }, [existing, reset]);

  
  const addLawyerSpecialization = () => {
    setLawyerSpecializationItems([...lawyerSpecializationItems, { 
      
      lawyerId: ,
      
      specializationId: ,
      
    }]);
  };

  const removeLawyerSpecialization = (index: number) => {
    setLawyerSpecializationItems(lawyerSpecializationItems.filter((_, i) => i !== index));
  };

  const updateLawyerSpecialization = (index: number, field: keyof LawyerSpecializationItem, value: any) => {
    const updated = [...lawyerSpecializationItems];
    updated[index] = { ...updated[index], [field]: value };
    setLawyerSpecializationItems(updated);
  };
  

  const onSubmit = async (data: FormValues) => {
    try {
      const payload = {
        ...data,
        
        lawyerSpecializations: lawyerSpecializationItems.filter(item => 
          
          item.lawyerId &&
          
          item.specializationId &&
          
          true
        ),
        
      };

      if (isEditing) {
        await updateMutation.mutateAsync({ id: id!, data: payload });
        toast.success("Specialization updated successfully");
      } else {
        await createMutation.mutateAsync(payload);
        toast.success("Specialization created successfully");
      }
      setLocation("/admin/specialization");
    } catch (error: any) {
      toast.error(error.message || "Failed to save specialization");
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
              onClick={() => setLocation("/admin/specialization")}
            >
              <ArrowLeft className="h-5 w-5" />
            </Button>
            <div>
              <h1 className="text-2xl font-bold tracking-tight">
                {isEditing ? "Edit Specialization" : "New Specialization"}
              </h1>
              <p className="text-muted-foreground">
                {isEditing ? "Update the details" : "Create a new specialization"}
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
                      <Label htmlFor="name">
                        Name *
                      </Label>
                      
                      <Input 
                        id="name" 
                        placeholder=""
                        {...register("name")} 
                      />
                      
                    </div>
                    
                    
                    <div className="space-y-2">
                      <Label htmlFor="description">
                        Description
                      </Label>
                      
                      <Textarea 
                        id="description" 
                        rows={4}
                        placeholder=""
                        {...register("description")} 
                      />
                      
                    </div>
                    
                  </div>
                </CardContent>
              </Card>

              
              {/* Child Entity Card */}
              <Card>
                <CardHeader className="flex flex-row items-center justify-between">
                  <CardTitle>Lawyers</CardTitle>
                  <Button type="button" variant="outline" size="sm" onClick={addLawyerSpecialization}>
                    <Plus className="h-4 w-4 mr-1" />
                    Add Item
                  </Button>
                </CardHeader>
                <CardContent>
                  <div className="space-y-3">
                    { (lawyerSpecializationItems || []).map((item, index) => (
                      <div key={index} className="p-4 border rounded-lg space-y-3 bg-muted/30">
                        <div className="flex gap-3 items-start">
                          <div className="flex-1 grid grid-cols-2 md:grid-cols-1 gap-3">
                            
                          </div>
                          { lawyerSpecializationItems.length > 1 && (
                            <Button 
                              type="button" 
                              variant="ghost" 
                              size="icon" 
                              onClick={() => removeLawyerSpecialization(index)}
                            >
                              <Trash2 className="h-4 w-4 text-destructive" />
                            </Button>
                          )}
                        </div>
                      </div>
                    ))}
                    { lawyerSpecializationItems.length === 0 && (
                      <div className="text-center py-8 text-muted-foreground">
                        No items yet. Click "Add Item" to start.
                      </div>
                    )}
                  </div>
                </CardContent>
              </Card>
              

              
              <Card>
                <CardHeader>
                  <CardTitle>Lawyers</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                    { (lawyerSpecializations as any)?.items?.map((item: any) => (
                      <div key={item.id} className="flex items-center space-x-2">
                        <Checkbox
                          id={`rel-${item.id}`}
                          checked={watch("lawyerSpecializationIds")?.includes(item.id)}
                          onCheckedChange={(checked) => {
                            const current = watch("lawyerSpecializationIds") || [];
                            if (checked) {
                              setValue("lawyerSpecializationIds", [...current, item.id]);
                            } else {
                              setValue("lawyerSpecializationIds", current.filter((id: string) => id !== item.id));
                            }
                          }}
                        />
                        <Label htmlFor={`rel-${item.id}`}>{item.name}</Label>
                      </div>
                    ))}
                    {!lawyerSpecializations?.items?.length && (
                      <div className="text-sm text-muted-foreground col-span-full">
                        No lawyerspecializations found.
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
                    {isEditing ? "Save Changes" : "Create Specialization"}
                  </Button>
                  <Button 
                    type="button" 
                    variant="outline" 
                    className="w-full"
                    onClick={() => setLocation("/admin/specialization")}
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
