import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLeadFormsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  leadWorkflowId?: string;
  }

export function useLeadForms(input: GetLeadFormsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["leadForms", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-form", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllLeadForms() {
  return useQuery({
    queryKey: ["leadForms", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-form", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLeadForm(id: string) {
  return useQuery({
    queryKey: ["leadForm", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lead-form/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLeadForm() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lead-form", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadForms"] });
    },
  });
}

export function useUpdateLeadForm() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lead-form/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["leadForms"] });
      queryClient.invalidateQueries({ queryKey: ["leadForm", data.id] });
    },
  });
}

export function useDeleteLeadForm() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lead-form/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadForms"] });
    },
  });
}


