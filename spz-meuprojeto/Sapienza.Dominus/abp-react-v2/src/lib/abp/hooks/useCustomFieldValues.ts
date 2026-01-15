import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetCustomFieldValuesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  customFieldId?: string;
  }

export function useCustomFieldValues(input: GetCustomFieldValuesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["customFieldValues", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/custom-field-value", {
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

export function useAllCustomFieldValues() {
  return useQuery({
    queryKey: ["customFieldValues", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/custom-field-value", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useCustomFieldValue(id: string) {
  return useQuery({
    queryKey: ["customFieldValue", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/custom-field-value/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateCustomFieldValue() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/custom-field-value", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["customFieldValues"] });
    },
  });
}

export function useUpdateCustomFieldValue() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/custom-field-value/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["customFieldValues"] });
      queryClient.invalidateQueries({ queryKey: ["customFieldValue", data.id] });
    },
  });
}

export function useDeleteCustomFieldValue() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/custom-field-value/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["customFieldValues"] });
    },
  });
}


